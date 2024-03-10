using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BingdingCommandTest
{
    class CancellationTokenClass
    {
        CancellationTokenSource? cts;
        ManualResetEvent mre = new ManualResetEvent(true);
        public string ContinueText { get; set; } = "继续";

        async void DoSomethingsInBackground(params object[] args)
        {
            var maxLoop = (int)args[0];
            var token = (CancellationToken)args[1];
            try
            {
                var t = Task.Run(() =>
                {
                    var loop = 0;
                    while (loop < maxLoop)
                    {
                        mre.WaitOne();
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine($"{loop} I'm stopped from user's operation");
                            return;
                        }

                        Console.WriteLine($"{loop} I'm working... ... => Loop = {loop}");
                        Task.Delay(1000).Wait();
                    }
                });
                await t;

                t.GetAwaiter().OnCompleted(() =>
                {
                    //ControlButton(isRunning: false);
                    Console.WriteLine("完成");
                });
            }
            catch { }
        }

        // 开始Command
        public void StartCommand()
        {
            cts = new CancellationTokenSource();
            DoSomethingsInBackground(500, cts.Token);
            //ControlButton(isRunning: true);
        }

        public void ContinueCommand() 
        {
            mre.Reset();
            if(ContinueText == "继续")
            {
                if(mre.Set())
                {
                    ContinueText = "暂停";
                }
                return;
            }
            ContinueText = "继续";
        }

        public void CancelCommand() 
        {
            mre.Reset();
            var ret = MessageBox.Show("Do you want to cancel?", "Cancel Task", MessageBoxButton.YesNo);
            if(ret == MessageBoxResult.Yes)
            {
                cts?.Cancel();
            }

            mre.Set();
        }
    }
}
