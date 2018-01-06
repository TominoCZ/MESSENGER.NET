using MESSENGER.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace MESSENGER
{
    public partial class MessengerForm : Form
    {
        private TcpClient _server;
        private NetworkStream _stream;
        private ClientUserAccount _account;

        Image image;

        int lastSelected;

        private bool loadedHistory;

        private bool wasTyping;

        public MessengerForm(string ip)
        {
            if (String.IsNullOrWhiteSpace(ip) || String.IsNullOrEmpty(ip))
            {
                MessageBox.Show("Please specify the server IP in the launch arguments like so:\n\nC:/.../MESENGER.exe -[ip:port]\n\nThe program will now close.");
                Process.GetCurrentProcess().Kill();
                return;
            }

            var loginForm = new LoginForm(ip);
            loginForm.ShowDialog();

            _server = loginForm._client;
            _stream = loginForm._stream;
            _account = loginForm._account;

            if (_account == null)
                Process.GetCurrentProcess().Kill();

            InitializeComponent();

            lblProfile.Text = _account.Nickname;

            pbProfile.Image = _account.GetProfileImage();

            pbProfile.AllowDrop = true;

            //listen for incomming data
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (_server.Connected && _stream.CanRead && _stream.DataAvailable)
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            onDataReceived(bf.Deserialize(_stream));
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(1);
                }
            })
            { IsBackground = true }.Start();
        }

        private void onDataReceived(object packet)
        {
            if (packet is List<ClientUserAccount> users)
            {
                //PRESERVE PREVIOUS DATA
                int selected = -1;
                Dictionary<int, List<ChatNode>> chats = new Dictionary<int, List<ChatNode>>();
                Dictionary<int, string> textsToSend = new Dictionary<int, string>();
                Dictionary<int, int> unread = new Dictionary<int, int>();
                Dictionary<int, bool> typing = new Dictionary<int, bool>();

                for (int i = 0; i < lbOnline.Items.Count; i++)
                {
                    var node = lbOnline.Items[i] as ClientUserAccount;

                    unread.Add(node.UUID, node.unread);
                    chats.Add(node.UUID, node.getChat());
                    typing.Add(node.UUID, node.isTyping);

                    if (node.Online)
                        textsToSend.Add(node.UUID, node.textToSend);
                }

                //LOAD HISTORY
                try
                {
                    if (!loadedHistory)
                    {
                        if (File.Exists($"MSGR_chat_history\\{_account.UUID}.dat"))
                        {
                            using (var fs = File.OpenRead($"MSGR_chat_history\\{_account.UUID}.dat"))
                            {
                                var bf = new BinaryFormatter();
                                UserChatHistory uch = bf.Deserialize(fs) as UserChatHistory;

                                if (uch.UUID == _account.UUID)
                                {
                                    chats = uch.chats;
                                    unread = uch.unread;

                                    selected = uch.lastSelected;
                                }
                            }
                        }

                        loadedHistory = true;
                    }
                }
                catch
                {
                }

                BeginInvoke(new MethodInvoker(() =>
                {
                    lbOnline.BeginUpdate();

                    if (lbOnline.SelectedItem != null)
                        selected = (lbOnline.SelectedItem as ClientUserAccount).UUID;

                    lbOnline.Items.Clear();

                    foreach (var client in users)
                    {
                        if (client.UUID == _account.UUID)
                            continue;

                        if (chats.TryGetValue(client.UUID, out var chat))
                            client.setChat(chat);

                        lbOnline.Items.Add(client);
                    }

                    if (lbOnline.Items.Count > 0)
                    {
                        try
                        {
                            if (selected != -1)
                                lbOnline.SelectedItem = getNodeWithID(selected);
                            else
                                lbOnline.SelectedIndex = 0;
                        }
                        catch
                        {
                        }
                        for (int i = 0; i < lbOnline.Items.Count; i++)
                        {
                            var node = lbOnline.Items[i] as ClientUserAccount;

                            unread.TryGetValue(node.UUID, out node.unread);
                            typing.TryGetValue(node.UUID, out node.isTyping);
                            textsToSend.TryGetValue(node.UUID, out node.textToSend);

                            if (lbOnline.SelectedItem is ClientUserAccount cua && cua.UUID == node.UUID)
                                tbMessage.Enabled = node.Online;
                        }
                    }
                    else
                    {
                        lbChat.Items.Clear();

                        tbMessage.Size = new Size(355, tbMessage.Size.Height);
                        pbToSend.Visible = false;
                        pbToSend.Image = null;
                        image = null;
                        tbMessage.Text = "";
                        lblChat.Text = "Chat:";
                    }

                    lbOnline.EndUpdate();
                }));
            }
            else if (packet is MessagePacket msgp)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    ClientUserAccount from = getNodeWithID(msgp.from);

                    if (from != null)
                    {
                        from.textToSend = msgp.message;
                        from.appendMessage(/*msgp.message, */from.Nickname, from.UUID, msgp.GetImage(), false);
                        from.textToSend = "";

                        if (from != lbOnline.SelectedItem)
                        {
                            from.unread++;

                            lbOnline.Invalidate();
                        }

                        updateChatBox(from);
                    }
                }));

                saveChatHistory();
            }
            else if (packet is LogoutAckPacket lop)
            {
                onDisconnected(lop.message);
            }
            else if (packet is TypingPacket tp)
            {
                var node = getNodeWithID(tp.from);

                if (node != null)
                {
                    node.isTyping = tp.typing;
                    lbOnline.Invalidate();
                }
            }
        }

        private void sendData(object toSend)
        {
            try
            {
                if (_stream.CanWrite)
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(_stream, toSend);
                }
            }
            catch { }
        }

        private void onDisconnected(string reason)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                lbOnline.Items.Clear();

                tsmiRemoveImage_Click(null, null);

                if (reason != "")
                    MessageBox.Show($"{reason}\n\nThe program will now close.", "Disconnected");
                else
                    MessageBox.Show("You've been disconnected from the server.\n\nThe program will now close.", "Disconnected");

                Application.Exit();
            }));
        }

        private void updateChatBox(ClientUserAccount account)
        {
            if (account == lbOnline.SelectedItem)
                account.viewedChat();

            if (account == null || account != lbOnline.SelectedItem)
                return;

            lbChat.Items.Clear();
            List<ChatNode> chat = account.getChat();

            foreach (var n in chat)
                lbChat.Items.Add(n);

            lbChat.TopIndex = lbChat.Items.Count - 1;
            lbChat.SelectedIndex = lbChat.Items.Count - 1;
            lbChat.SelectedIndex = -1;
        }

        private void lbOnline_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbMessage.Enabled = lbOnline.SelectedItem != null;

            try
            {
                if (lbOnline.SelectedIndex == -1)
                {
                    if (lbOnline.SelectedIndex == lastSelected)
                        lblChat.Text = "Chat:";
                    else
                        lbOnline.SelectedIndex = lastSelected;
                }
                else
                {
                    lastSelected = lbOnline.SelectedIndex;

                    var node = lbOnline.SelectedItem as ClientUserAccount;
                    updateChatBox(node);
                    node.viewedChat();

                    tbMessage.Enabled = node.Online;
                    tbMessage.Text = node.textToSend;

                    lblChat.Text = $"Chat with {node.Nickname}:";

                    lbOnline.Invalidate();
                }
            }
            catch
            {
            }
        }

        private ClientUserAccount getNodeWithID(int ID)
        {
            foreach (ClientUserAccount user in lbOnline.Items)
            {
                if (user.UUID == ID)
                    return user;
            }

            return null;
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            bool isTyping = lbOnline.SelectedIndex != -1 &&
                           (!String.IsNullOrEmpty(tbMessage.Text) && !String.IsNullOrWhiteSpace(tbMessage.Text) &&
                            tbMessage.Text.Length > 0 || image != null);

            if (!e.Shift && e.KeyCode == Keys.Enter)
            {
                if (isTyping)
                {
                    if (lbOnline.SelectedItem is ClientUserAccount node)
                    {
                        sendData(new MessagePacket(tbMessage.Text, _account.UUID, node.UUID, image));

                        saveChatHistory();

                        node.appendMessage(_account.Nickname, _account.UUID, image, true);
                        node.textToSend = "";
                        tbMessage.Text = "";

                        updateChatBox(node);

                        tbMessage.Size = new Size(355, tbMessage.Size.Height);
                        pbToSend.Visible = false;
                        pbToSend.Image = null;

                        image = null;
                    }
                }

                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Back && tbMessage.Text == "" && image != null)
            {
                tsmiRemoveImage_Click(null, null);

                if (lbOnline.SelectedItem is ClientUserAccount acc)
                    sendData(new TypingPacket(_account.UUID, acc.UUID, false));
            }
            if (!e.Shift && e.KeyCode == Keys.Enter && isTyping)
                if (lbOnline.SelectedItem is ClientUserAccount acc)
                    sendData(new TypingPacket(_account.UUID, acc.UUID, false));

            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    if (lbOnline.SelectedItem != null)
                    {
                        var list = Clipboard.GetFileDropList();
                        try
                        {
                            Image img;

                            if (list != null && list.Count > 0)
                            {
                                img = Image.FromFile(list[0]);
                            }
                            else
                                img = Clipboard.GetImage();

                            if (img != null)
                            {
                                tbMessage.Size = new Size(269, tbMessage.Size.Height);
                                pbToSend.Image = image = img;
                                pbToSend.Visible = true;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (e.KeyCode == Keys.A)
                {
                    tbMessage.SelectionStart = 0;
                    tbMessage.SelectionLength = tbMessage.Text.Length;
                }
            }
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool isTyping = lbOnline.SelectedIndex != -1 &&
                                (!String.IsNullOrEmpty(tbMessage.Text) && !String.IsNullOrWhiteSpace(tbMessage.Text) &&
                                 tbMessage.Text.Length > 0 || image != null);

                if (isTyping != wasTyping)
                    sendData(new TypingPacket(_account.UUID, (lbOnline.Items[lastSelected] as ClientUserAccount).UUID,
                        isTyping));

                wasTyping = isTyping;

                if (lbOnline.SelectedItem is ClientUserAccount acc)
                {
                    acc.textToSend = tbMessage.Text;
                }
            }
            catch { }
        }

        private void tbMessage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tbMessage_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (lbOnline.SelectedItem is ClientUserAccount acc)
                {
                    image = Image.FromFile(files[0]);

                    tbMessage.Size = new Size(269, tbMessage.Size.Height);
                    pbToSend.Image = image;
                    pbToSend.Visible = true;

                    sendData(new TypingPacket(_account.UUID, acc.UUID, true));
                }
            }
            catch
            {

            }
        }

        private void lbChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (lbChat.SelectedItem != null)
                    Clipboard.SetText(lbChat.SelectedItem.ToString());
            }
        }

        private void lbChat_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (lbChat.Items.Count > 0 && e.Index != -1)
            {
                try
                {
                    DrawingHelper.enableSmooth(e.Graphics, false);

                    ChatNode node = lbChat.Items[e.Index] as ChatNode;

                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e = new DrawItemEventArgs(e.Graphics,
                            e.Font,
                            e.Bounds,
                            e.Index,
                            e.State ^ DrawItemState.Selected,
                            e.ForeColor,
                            Color.FromArgb(0, 180, 255));

                        e.DrawBackground();
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        e.DrawBackground();
                        e.DrawFocusRectangle();

                        e.Graphics.FillRectangle(new SolidBrush(node.currentUser ? Color.FromArgb(192, 237, 252) : Color.FromArgb(240, 244, 248)), e.Bounds);
                    }

                    DrawingHelper.enableSmooth(e.Graphics, true);

                    e.Graphics.DrawString(node.ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);

                    if (node.image != null)
                    {
                        Image cropped = ImageUtils.ResizeAndCrop(node.image, 150, 100);

                        e.Graphics.TranslateTransform(0, lbChat.GetItemHeight(e.Index) - 100);
                        e.Graphics.DrawImage(cropped, 0, e.Bounds.Y);
                    }
                }
                catch { }
            }
        }

        private void lbChat_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            try
            {
                ChatNode chatNode = lbChat.Items[e.Index] as ChatNode;

                e.ItemHeight = (int)e.Graphics.MeasureString(chatNode.ToString(), lbChat.Font, lbChat.Width).Height;

                if (chatNode.image != null)
                    e.ItemHeight += 100;
            }
            catch
            {
            }
        }

        private void lbOnline_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (lbOnline.Items.Count > 0 && e.Index != -1)
            {
                ClientUserAccount account = lbOnline.Items[e.Index] as ClientUserAccount;

                DrawingHelper.enableSmooth(e.Graphics, false);

                e.DrawBackground();

                if (account.unread > 0)
                    e.Graphics.FillRectangle(Brushes.Orange, e.Bounds);
                else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e = new DrawItemEventArgs(e.Graphics,
                        e.Font,
                        e.Bounds,
                        e.Index,
                        e.State ^ DrawItemState.Selected,
                        e.ForeColor,
                        Color.FromArgb(0, 180, 255));

                    e.DrawBackground();
                    e.DrawFocusRectangle();
                }

                Brush brush = account.banned ? new SolidBrush(Color.FromArgb(255, 50, 100)) : new SolidBrush(account.Online ? Color.FromArgb(150, 255, 50) : Color.FromArgb(50, 50, 50));

                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Width - 5, e.Bounds.Y, 5, 50));

                Image img = ImageUtils.ResizeAndCrop(account.GetProfileImage(), 50, 50);

                DrawingHelper.enableSmooth(e.Graphics, true);

                e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y);

                if (account.unread > 0)
                {
                    DrawingHelper.enableSmooth(e.Graphics, false);
                    e.Graphics.FillRectangle(Brushes.Black, 0, e.Bounds.Y, 20, 20);
                    e.Graphics.FillRectangle(Brushes.Orange, 0, e.Bounds.Y, 19, 19);
                    DrawingHelper.enableSmooth(e.Graphics, true);

                    Font f = new Font(lbOnline.Font.FontFamily, 12f, FontStyle.Bold);

                    string text = $"{account.unread}";

                    SizeF size = e.Graphics.MeasureString(text, f, new Size(50, 50));

                    e.Graphics.DrawString(text, f, Brushes.Black, 9 - size.Width / 2f, e.Bounds.Y + (9 - size.Height / 2f) + 0.75f);
                }
                if (account.isTyping)
                {
                    Font f = new Font(lbOnline.Font.FontFamily, 21, FontStyle.Bold);

                    string text = "...";

                    SizeF s = e.Graphics.MeasureString(text, f, new Size(50, 50));

                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), 10, e.Bounds.Y + 50 - 10, 50 - 20, 10);

                    e.Graphics.DrawString(text, f, Brushes.Turquoise, 25 - s.Width / 2f + 1, e.Bounds.Y + 25 - s.Height / 2f + 14);
                }

                int i = (int)e.Graphics.MeasureString(account.ToString(), lbOnline.Font, lbOnline.Width - 60).Height;
                e.Graphics.DrawString(account.ToString(), e.Font, new SolidBrush(Color.Black), new Rectangle(new Point(55, e.Bounds.Y + 25 - i / 2), new Size(e.Bounds.Width - 60, 50)));
            }
        }

        private void lbChat_Leave(object sender, EventArgs e)
        {
            lbChat.SelectedIndex = -1;
        }

        private void tsmiRemoveImage_Click(object sender, EventArgs e)
        {
            tbMessage.Size = new Size(355, tbMessage.Size.Height);
            pbToSend.Visible = false;
            pbToSend.Image = null;
            image = null;
        }

        private void pbProfile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pbProfile_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                pbProfile.Image = ImageUtils.ResizeAndCrop(Image.FromFile(files[0]), 50, 50);

                pbProfile.Invalidate();

                sendData(new ProfileUpdatePacket(_account.Nickname, pbProfile.Image));
            }
            catch
            {

            }
        }

        private void tsmiCopyImage_Click(object sender, EventArgs e)
        {
            var node = lbChat.SelectedItem as ChatNode;

            if (node?.image != null)
                Clipboard.SetImage(node.image);
        }

        private void cmsChatImage_Opening(object sender, CancelEventArgs e)
        {
            if (lbChat.SelectedItem is ChatNode node)
            {
                tsmiCopyImage.Enabled = tsmiSaveImage.Enabled = node.image != null;

                tsmiCopyMessage.Enabled = !String.IsNullOrEmpty(node.message);
            }
            else
                e.Cancel = true;
        }

        private void tsmiSaveImage_Click(object sender, EventArgs e)
        {
            var node = lbChat.SelectedItem as ChatNode;

            if (node?.image != null)
            {
                try
                {
                    string ext = new ImageFormatConverter().ConvertToString(node.image.RawFormat)?.ToLower();

                    var fopd = new SaveFileDialog();
                    fopd.Title = "Save image as..";
                    fopd.Filter = $"Image Files (*.{ext})|*.{ext}";
                    fopd.DefaultExt = ext;
                    var res = fopd.ShowDialog();

                    if (res == DialogResult.OK)
                        node.image.Save(fopd.FileName, node.image.RawFormat);
                }
                catch { }
            }
        }

        private void tsmiCopyMessage_Click(object sender, EventArgs e)
        {
            if (lbChat.SelectedItem is ChatNode node)
                Clipboard.SetText(node.message);
        }

        private void tsmiEditName_Click(object sender, EventArgs e)
        {
            var form = new EditNameForm(_account.Nickname);

            form.ShowDialog();

            _account.Nickname = form.newName;
            lblProfile.Text = form.newName;

            sendData(new ProfileUpdatePacket(_account.Nickname, pbProfile.Image));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool hasUnreadMessages = false;

            for (int i = 0; i < lbOnline.Items.Count; i++)
            {
                if (lbOnline.Items[i] is ClientUserAccount acc)
                {
                    if (acc.unread > 0)
                    {
                        hasUnreadMessages = true;
                        break;
                    }
                }
            }

            if (hasUnreadMessages)
            {
                var result = MessageBox.Show("You have some unread messages.\nAre you sure you want to exit?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            for (int i = 0; i < lbOnline.Items.Count; i++)
            {
                if (lbOnline.Items[i] is ClientUserAccount acc)
                {
                    sendData(new TypingPacket(_account.UUID, acc.UUID, false));
                }
            }

            try
            {
                if (_server != null)
                {
                    try
                    {
                        sendData(new LogoutAckPacket());
                        _server.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            Visible = false;
            Thread.Sleep(500);

            saveChatHistory();
        }

        private void saveChatHistory()
        {
            try
            {
                Dictionary<int, List<ChatNode>> chatHistory = new Dictionary<int, List<ChatNode>>();
                Dictionary<int, int> unread = new Dictionary<int, int>();

                for (int i = 0; i < lbOnline.Items.Count; i++)
                {
                    var user = lbOnline.Items[i] as ClientUserAccount;

                    chatHistory.Add(user.UUID, user.getChat());
                    unread.Add(user.UUID, user.unread);
                }

                var bf = new BinaryFormatter();

                if (!Directory.Exists("MSGR_chat_history"))
                    Directory.CreateDirectory("MSGR_chat_history");
                else if (File.Exists($"MSGR_chat_history\\{ _account.UUID}.dat"))
                    File.Delete($"MSGR_chat_history\\{_account.UUID}.dat");

                using (var fs = File.OpenWrite($"MSGR_chat_history\\{_account.UUID}.dat"))
                {
                    int selected = -1;

                    if (lbOnline.SelectedItem is ClientUserAccount cua)
                        selected = cua.UUID;

                    bf.Serialize(fs, new UserChatHistory(_account.UUID, chatHistory, unread, selected));
                }
            }
            catch
            {
            }
        }
    }

    [Serializable]
    class UserChatHistory
    {
        public int UUID { get; }

        public Dictionary<int, List<ChatNode>> chats { get; }
        public Dictionary<int, int> unread { get; }

        public int lastSelected { get; }

        public UserChatHistory(int uuid, Dictionary<int, List<ChatNode>> chats, Dictionary<int, int> unread, int lastSelected)
        {
            UUID = uuid;
            this.chats = chats;
            this.unread = unread;
            this.lastSelected = lastSelected;
        }
    }
}
