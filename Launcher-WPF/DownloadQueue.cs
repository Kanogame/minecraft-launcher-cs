using BackendCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Launcher_WPF
{
    public class DownloadQueue
    {
        private string CurrentTask;
        private string path;
        private List<string> Tasks;

        FileRequest fileRequest;


        public DownloadQueue(IEnumerable<string> instanceNames, string path , goConn GoConn, ProgressBar pb)
        {
            fileRequest = new FileRequest(GoConn, pb);
            fileRequest.DownloadCompleted += FileRequest_DownloadCompleted;

            Tasks = instanceNames.ToList();
            this.path = path;
            TaskManager();
        }

        private void FileRequest_DownloadCompleted(string instanceName)
        {
            if (CurrentTask == instanceName)
            {
                RemoveTask(instanceName);
                TaskManager();
            }
        }

        private void RemoveTask(string instanceName)
        {
            Tasks.Remove(instanceName);
        }

        private string GetTask()
        {
            if (Tasks != null)
            {
                return Tasks[0];
            }
            return null;
        }

        private void TaskManager()
        {
            if (GetTask() != null)
            {
                CurrentTask = GetTask();
                fileRequest.GetFile("PlaceHoler", CurrentTask, path);
            }
        }
    }
}
