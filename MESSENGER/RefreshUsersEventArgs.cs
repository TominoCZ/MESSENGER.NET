using System;
using System.Collections;
using System.Collections.Generic;

namespace MESSENGER
{
    public class RefreshUsersEventArgs : EventArgs
    {
        public List<ClientUserAccount> nodes;

        public RefreshUsersEventArgs(List<ClientUserAccount> nodes)
        {
            this.nodes = nodes;
        }
    }
}