using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingdingCommandTest
{
    public class SemaphoreClass
    {
        private static Semaphore? semaphore;
        private static int interval;

        public void RumMultipleThread()
        {
            semaphore = new Semaphore(0, 3);
            var threads = new List<Thread>();

            for(int i = 0; i < 5; i++) 
            {
                Thread t = new Thread(() => { TestSemaphore(); })
                {
                    Name = $"线程-{i}"
                };
                t.Start();
                Thread.Sleep(100);
                threads.Add(t);
            }

            Thread.Sleep(500);
            var releaseCount = 3;
            Console.WriteLine($"主线程释放{releaseCount}个信号");
            semaphore.Release(releaseCount: releaseCount);

            while (threads.Any(t => t.IsAlive)) ;
            Console.WriteLine("所有线程运行结束");
        }

        private void TestSemaphore()
        {
            Console.WriteLine($"{LogInfo}开始等待信号量");
            semaphore!.WaitOne();

            int padding = Interlocked.Add(ref interval, 100);

            Console.WriteLine($"{LogInfo}获得信号量，做些事情...");
            Thread.Sleep(1000 + padding);
            Console.WriteLine($"{LogInfo}结束 => 释放信号量，之前的信号量数：{semaphore!.Release()}");
        }

        string LogInfo => $"{Now}[{Thread.CurrentThread.Name}]";
        string Now => $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}]";
    }
}
