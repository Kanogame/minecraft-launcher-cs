using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackendCommon
{
    public class ProducerConsumerQueue<T> : IDisposable 
        where T : class
    {
      EventWaitHandle wh = new AutoResetEvent(false);
      Thread worker;
      object locker = new object();
      Queue<T> tasks = new Queue<T>();
      Action<T> processTask;
 
      public ProducerConsumerQueue(Action<T> processTask) 
      {
        this.processTask = processTask;
        worker = new Thread(Work);
        worker.IsBackground = true;
        worker.Start();
      }
 
      public void EnqueueTask(T task)
      {
        lock (locker)
          tasks.Enqueue(task);

        wh.Set();
      }
 
      public void Dispose() 
      {
        EnqueueTask(null);      // Сигнал Потребителю на завершение
        worker.Join();          // Ожидание завершения Потребителя
        wh.Close();             // Освобождение ресурсов
      }
 
      void Work() 
      {
        while (true) 
        {
          T task = null;

          lock (locker)
          {
            if (tasks.Count > 0) 
            {
              task = tasks.Dequeue();
              if (task == null)
                return;
            }
          }

          if (task != null) 
          {
            processTask(task);
          }
          else
            wh.WaitOne();
        }
      }
    }
}
