using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BingdingCommandTest
{
    public class Mycommand : ICommand
    {
        private Action action;
        public Mycommand(Action action)
        { 
            this.action = action; 
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (action != null)
            {
                action();
            };
        }
    }
}
