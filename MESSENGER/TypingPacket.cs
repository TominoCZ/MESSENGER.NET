using System;

namespace MESSENGER
{
    [Serializable]
    internal class TypingPacket
    {
        public int from;
        public int to;
        public bool typing;

        public TypingPacket(int from, int to, bool typing)
        {
            this.from = from;
            this.to = to;
            this.typing = typing;
        }
    }
}