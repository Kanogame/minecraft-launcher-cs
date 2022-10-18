using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_Backend
{
    class TaskRemoveComputer : TaskToClient
    {
        public computer SendTo;
        public Guid whoRemoved;

        public TaskRemoveComputer(computer SendTo, Guid whoRemoved)
        {
            this.SendTo = SendTo;
            this.whoRemoved = whoRemoved;
        }
    }
}
