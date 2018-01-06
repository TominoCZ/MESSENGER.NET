using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MESSENGER
{
    [Serializable]
    class ProfileUpdatePacket
    {
        public string Nickname;
        private byte[] profileImage;

        public ProfileUpdatePacket(string nickname, Image img)
        {
            Nickname = nickname;

            profileImage = ImageUtils.GetBytes(img);
        }

        public Image GetProfileImage()
        {
            return ImageUtils.GetImage(profileImage);
        }
    }
}