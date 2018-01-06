using System;
using System.Drawing;
using System.IO;

namespace MESSENGER
{
    [Serializable]
    class MessagePacket
    {
        public string message;
        public int to, from;

        private byte[] image;

        public MessagePacket(string message, int from, int to, Image img)
        {
            this.message = message;
            this.from = from;
            this.to = to;

            if (img != null)
            {
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    image = ms.ToArray();
                }
            }
        }

        public Image GetImage()
        {
            if (image == null)
                return null;

            return Image.FromStream(new MemoryStream(image));
        }
    }
}