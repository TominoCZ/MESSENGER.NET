using System.Net.Sockets;

namespace MESSENGER
{
    class UserConnection
    {
        public TcpClient client;
        public NetworkStream stream;
        public UserAccount account;
        public bool loggedIn;
        public bool isTyping;

        public UserConnection(TcpClient client, UserAccount account)
        {
            this.client = client;
            this.account = account;

            stream = client.GetStream();
        }
    }
}