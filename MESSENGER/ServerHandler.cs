using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MESSENGER
{
    public class ServerHandler
    {
        public EventHandler<RefreshUsersEventArgs> OnUsersChanged;

        private TcpListener listener;

        private ArrayList registered;

        private ArrayList connected;

        private List<string> LOG;

        public ServerHandler(int port)
        {
            registered = ArrayList.Synchronized(new ArrayList());
            connected = ArrayList.Synchronized(new ArrayList());

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            LOG = new List<string>();

            log("Server started.");
        }

        public void init()
        {
            try
            {
                var bf = new BinaryFormatter();

                if (File.Exists("accounts.dat"))
                {
                    using (var stream = File.OpenRead("accounts.dat"))
                    {
                        registered = bf.Deserialize(stream) as ArrayList;
                    }

                    sendUsersToAll();
                }
            }
            catch { }

            //remove disconnected users
            new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            for (int i = 0; i < connected.Count; i++)
                            {
                                if (connected[i] is UserConnection c && (!c.client.Connected || !c.client.Client.Connected))
                                    connected.Remove(c);
                            }
                        }
                        catch { }

                        Thread.Sleep(1);
                    }
                })
            { IsBackground = true }.Start();

            //accept connections
            new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            connected.Add(new UserConnection(client, null));
                        }
                        catch
                        {
                            Thread.Sleep(100);
                        }
                    }
                })
            { IsBackground = true }.Start();

            //accept data
            new Thread(() =>
                {
                    while (true)
                    {
                        for (int i = 0; i < connected.Count; i++)
                        {
                            UserConnection connectedUser = null;

                            try
                            {
                                connectedUser = connected[i] as UserConnection;
                            }
                            catch
                            {
                            }

                            if (connectedUser != null && connectedUser.stream.DataAvailable && connectedUser.stream.CanRead)
                                receivedData(connectedUser);
                        }

                        Thread.Sleep(1);
                    }
                })
            { IsBackground = true }.Start();
        }

        private void receivedData(UserConnection from)
        {
            BinaryFormatter bf = new BinaryFormatter();
            object received = bf.Deserialize(from.stream);

            if (received is LoginRequestPacket lrp)
            {
                UserAccount acc = getRegisteredUserAccount(lrp.LoginName, lrp.LoginPassword);

                if (acc != null && !from.loggedIn && getUserById(acc.UUID) == null)
                {
                    if (acc.banned)
                    {
                        bf.Serialize(from.stream, new LoginFailedPacket("You are banned from this server."));
                        connected.Remove(getUserById(acc.UUID));
                        return;
                    }

                    from.loggedIn = true;
                    from.account = acc;

                    bf.Serialize(from.stream, new ClientUserAccount(acc.Nickname, acc.UUID, true, acc.banned, acc.GetProfileImage()));

                    loggedIn(from);

                    log(acc.Nickname + " has logged in.");
                }
                else
                    bf.Serialize(from.stream, new LoginFailedPacket(acc != null ? "Already logged in" : "This account does not exist."));
            }
            else if (received is RegisterRequestPacket rrp)
            {
                UserAccount acc = getRegisteredUserAccount(rrp.LoginName);

                if (acc == null)
                {
                    acc = new UserAccount(rrp.LoginName, rrp.LoginPassword, rrp.Nickname, registered.Count);

                    from.loggedIn = true;
                    from.account = acc;

                    registered.Add(acc);

                    bf.Serialize(from.stream, new ClientUserAccount(acc.Nickname, acc.UUID, true, acc.banned, acc.GetProfileImage()));

                    log(acc.Nickname + " has registered an account.");

                    loggedIn(from);
                }
                else
                    bf.Serialize(from.stream, new RegisterFailedPacket());
            }
            else if (received is LogoutAckPacket)
            {
                loggedOut(from);
            }
            else if (received is MessagePacket msgp)
            {
                var user = getUserById(msgp.to);

                if (user != null)
                {
                    bf.Serialize(user.stream, msgp);

                    log($"({getUserById(msgp.from).account.Nickname} -> {user.account.Nickname}): {msgp.message}");
                }
            }
            else if (received is ProfileUpdatePacket pup)
            {
                from.account.Nickname = pup.Nickname;
                from.account.SetProfileImage(pup.GetProfileImage());

                sendUsersToAll();

                log(from.account.Nickname + " has updated his profile.");
            }
            else if (received is TypingPacket tp)
            {
                try
                {
                    if (getUserById(tp.from) is UserConnection c)
                        c.isTyping = tp.typing;

                    bf.Serialize(getUserById(tp.to).stream, tp);

                    List<ClientUserAccount> accounts = new List<ClientUserAccount>();

                    for (int i = 0; i < registered.Count; i++)
                    {
                        try
                        {
                            var user = registered[i] as UserAccount;
                            var connection = getUserById(user.UUID);

                            accounts.Add(new ClientUserAccount(user.Nickname, user.UUID, connection != null && connection.loggedIn, user.banned, user.GetProfileImage()) { isTyping = connection != null && connection.isTyping });
                        }
                        catch
                        {
                        }
                    }

                    OnUsersChanged(this, new RefreshUsersEventArgs(accounts));
                }
                catch
                {

                }
            }
        }

        private void sendUsersToAll()
        {
            BinaryFormatter bf = new BinaryFormatter();

            List<ClientUserAccount> accounts = new List<ClientUserAccount>();

            for (int i = 0; i < registered.Count; i++)
            {
                try
                {
                    var user = registered[i] as UserAccount;
                    var connection = getUserById(user.UUID);

                    var online = connection != null && connection.loggedIn;

                    accounts.Add(new ClientUserAccount(user.Nickname, user.UUID, online, user.banned, user.GetProfileImage()) { isTyping = connection != null && connection.isTyping });
                }
                catch
                {
                }
            }

            for (int i = 0; i < connected.Count; i++)
            {
                try
                {
                    var user = connected[i] as UserConnection;

                    bf.Serialize(user.stream, accounts);
                }
                catch
                {
                }
            }

            OnUsersChanged(this, new RefreshUsersEventArgs(accounts));
        }

        private void loggedIn(UserConnection user)
        {
            sendUsersToAll();

            if (user.account != null)
                log(user.account.Nickname + " logged in.");
        }

        private void loggedOut(UserConnection user)
        {
            connected.Remove(user);
            sendUsersToAll();

            if (user.account != null)
                log(user.account.Nickname + " logged out.");
        }

        public void Kick(int UUID, string text)
        {
            try
            {
                var user = getUserById(UUID);

                var bf = new BinaryFormatter();
                bf.Serialize(user.stream, new LogoutAckPacket(text));

                connected.Remove(user);

                log(user.account.Nickname + " was kicked from the server" + (text.Length == 0 ? "." : ": " + text + "."));
            }
            catch
            {
            }

            sendUsersToAll();
        }

        public void Kick(int UUID)
        {
            Kick(UUID, "");
        }

        public void banAccount(int UUID)
        {
            getRegisteredUserAccount(UUID).banned = true;
            Kick(UUID, "You have been banned from this server.");
        }

        public void unbanAccount(int UUID)
        {
            getRegisteredUserAccount(UUID).banned = false;
            sendUsersToAll();
        }

        public void unregisterAccount(int UUID)
        {
            registered.Remove(getRegisteredUserAccount(UUID));
            Kick(UUID, "Your account has been deleted.");
        }

        public void Dispose()
        {
            if (File.Exists("accounts.dat"))
                File.Delete("accounts.dat");

            var bf = new BinaryFormatter();

            using (var stream = File.OpenWrite("accounts.dat"))
            {
                bf.Serialize(stream, registered);
            }

            for (int i = 0; i < connected.Count; i++)
            {
                var user = connected[i] as UserConnection;
                bf.Serialize(user.stream, new LogoutAckPacket("The server has been shut down."));
            }

            listener.Stop();

            log("Server stopped.");

            writeLog();
        }

        UserConnection getUserById(int ID)
        {
            for (int i = 0; i < connected.Count; i++)
            {
                var user = connected[i] as UserConnection;

                if (user.account != null && user.account.UUID == ID)
                    return user;
            }

            return null;
        }

        UserAccount getRegisteredUserAccount(string loginName, string loginPassword)
        {
            try
            {
                for (int i = 0; i < registered.Count; i++)
                {
                    var acc = registered[i] as UserAccount;

                    if (acc.LoginName == loginName && acc.LoginPassword == loginPassword)
                        return acc;
                }
            }
            catch { }

            return null;
        }

        UserAccount getRegisteredUserAccount(string loginName)
        {
            try
            {
                for (int i = 0; i < registered.Count; i++)
                {
                    var acc = registered[i] as UserAccount;

                    if (acc.LoginName == loginName)
                        return acc;
                }
            }
            catch { }

            return null;
        }

        UserAccount getRegisteredUserAccount(int UUID)
        {
            try
            {
                for (int i = 0; i < registered.Count; i++)
                {
                    var acc = registered[i] as UserAccount;

                    if (acc.UUID == UUID)
                        return acc;
                }
            }
            catch { }

            return null;
        }

        private void log(string text)
        {
            LOG.Add($"[{DateTime.Now.ToShortTimeString()}] {text}");
        }

        public void writeLog()
        {
            string file;

            if (!Directory.Exists("server-logs"))
                Directory.CreateDirectory("server-logs");

            for (int i = 0; i < int.MaxValue; i++)
            {
                if (!File.Exists(file = $"server-logs\\server-log_{i}.log"))
                {
                    File.WriteAllLines(file, LOG.ToArray());
                    break;
                }
            }
        }
    }
}