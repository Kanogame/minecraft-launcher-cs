using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_Backend
{
    class TaskAddComputer : TaskToClient
    {
        public computer SendTo;
        public computer whoAdded;

        public TaskAddComputer(computer SendTo, computer whoAdded)
        {
            this.SendTo = SendTo;
            this.whoAdded = whoAdded;
        }
    }
}
