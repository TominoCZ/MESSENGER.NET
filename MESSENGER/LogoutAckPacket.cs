using System;

namespace MESSENGER
{
    [Serializable]
    class LogoutAckPacket
    {
        public string message;

        public LogoutAckPacket()
        {
            
        }

        public LogoutAckPacket(string message)
        {
            this.message = message;
        }
    }
}