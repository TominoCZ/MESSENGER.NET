using System;

namespace MESSENGER
{
    [Serializable]
    class LoginRequestPacket
    {
        public string LoginName;
        public string LoginPassword;

        public LoginRequestPacket(string loginName, string loginPassword)
        {
            LoginName = loginName;
            LoginPassword = loginPassword;
        }
    }
}