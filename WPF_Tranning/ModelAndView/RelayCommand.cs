using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BaseBallGame_WPF
{
    public class RelayCommand : ICommand

    {

        private Action<object> execute;

        private Func<object, bool> canExecute;



        public event EventHandler CanExecuteChanged

        {

            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }

        }



        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)

        {

            this.execute = execute;

            this.canExecute = canExecute;

        }



        public bool CanExecute(object parameter)

        {
            return this.canExecute == null || this.canExecute(parameter);
        }



        public void Execute(object parameter)

        {
            // MessageBox.Show("클릭 Execute");
            this.execute(parameter ?? "널이얌"); // 앙 파라메터띠
        }

    }

}