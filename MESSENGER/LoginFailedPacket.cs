using System;

namespace MESSENGER
{
    [Serializable]
    internal class LoginFailedPacket
    {
        public string reason;

        public LoginFailedPacket(string reason)
        {
            this.reason = reason;
        }
    }
}