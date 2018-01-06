using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MESSENGER.Properties;

namespace MESSENGER
{
    [Serializable]
    class UserAccount
    {
        public string LoginName { get; }

        public string LoginPassword { get; }

        public string Nickname;

        public int UUID { get; }

        private byte[] profileImage;

        public bool banned;

        public UserAccount(string loginName, string loginPassword, string nickname, int nth)
        {
            LoginName = loginName;
            LoginPassword = loginPassword;

            Nickname = nickname;

            UUID = (loginName + LoginPassword + nth).GetHashCode();
        }

        public void SetProfileImage(Image img)
        {
            profileImage = ImageUtils.GetBytes(img);
        }

        public Image GetProfileImage()
        {
            return profileImage != null ? ImageUtils.GetImage(profileImage) : Resources.unknown;
        }
    }
}