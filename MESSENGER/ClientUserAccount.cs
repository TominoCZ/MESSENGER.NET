using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using MESSENGER.Properties;

namespace MESSENGER
{
    [Serializable]
    public class ClientUserAccount
    {
        public string Nickname;

        public int UUID { get; }

        public bool Online;

        private byte[] profileImage;

        private List<ChatNode> chat;

        public int unread;

        public string textToSend;

        private DateTime lastShownTime;

        public bool banned { get; }

        public bool isTyping;

        public ClientUserAccount(string nickname, int uuid, bool online, bool banned, Image img)
        {
            Nickname = nickname;
            UUID = uuid;

            Online = online;

            this.banned = banned;

            chat = new List<ChatNode>();

            lastShownTime = DateTime.MinValue;

            profileImage = ImageUtils.GetBytes(img);
        }

        public ClientUserAccount(int uUID)
        {
            UUID = uUID;
        }

        public List<ChatNode> getChat()
        {
            return chat;
        }

        public void setChat(List<ChatNode> chat)
        {
            this.chat = chat;
        }

        public Image GetProfileImage()
        {
            return profileImage != null ? ImageUtils.GetImage(profileImage) : Resources.unknown;
        }

        public void appendMessage(/*string message, */string nick, int senderUUID, Image image, bool currentUser)
        {
            bool showTime = true;

            var now = DateTime.Now;

            if (chat.Count > 0)
            {
                double difference = new DateTime(now.Ticks, now.Kind).Subtract(lastShownTime).TotalMinutes;

                if (showTime = (chat.Last().senderUUID != senderUUID || difference > 1))
                    lastShownTime = new DateTime(now.Ticks, now.Kind).Subtract(TimeSpan.FromSeconds(now.Second));
            }
            else
                lastShownTime = new DateTime(now.Ticks, now.Kind).Subtract(TimeSpan.FromSeconds(now.Second));

            chat.Add(new ChatNode(textToSend, nick, image, senderUUID, showTime, currentUser));
        }

        public override string ToString()
        {
            return $"{Nickname}"; // {(unread > 0 ? $"[{(unread > 10 ? ">10" : $"{unread}")}]" : "")}" + (isTyping ? " [...]" : "");
        }

        public void viewedChat()
        {
            unread = 0;
        }
    }
}