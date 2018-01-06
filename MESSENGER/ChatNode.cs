using System;
using System.Drawing;

namespace MESSENGER
{
    [Serializable]
    public class ChatNode
    {
        public bool currentUser;

        public int senderUUID;
        public string message;
        private string sender;

        public Image image;

        public DateTime timeSent;

        private readonly bool showTime;

        public ChatNode(string message, string sender, Image image, int senderUUID, bool showTime, bool currentUser)
        {
            this.message = message;
            this.sender = sender;
            this.senderUUID = senderUUID;

            this.image = image;

            this.currentUser = currentUser;

            this.showTime = showTime;

            timeSent = DateTime.Now;
        }

        public override string ToString()
        {
            string msg = message;

            if (showTime)
                msg = $"[{timeSent.ToShortTimeString()}] {sender}:\n" + msg;

            return msg;
        }
    }
}