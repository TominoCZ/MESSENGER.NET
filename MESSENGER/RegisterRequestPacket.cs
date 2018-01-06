using System;

namespace MESSENGER
{
    [Serializable]
    class RegisterRequestPacket
    {
        public string LoginName;
        public string LoginPassword;

        public string Nickname;

        public RegisterRequestPacket(string loginName, string loginPassword, string nickname)
        {
            LoginName = loginName;
            LoginPassword = loginPassword;

            Nickname = nickname;
        }
    }
}