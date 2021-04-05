using BaseBallGame_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPF_Tranning
{
    public class EnterGame : IBaseCommand
    {


        private Action<object> execute;

        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public EnterGame()
        {

        }
        public EnterGame(Action<object> execute, Func<object, bool> canExecute = null)

        {

            this.execute = execute;
            this.canExecute = canExecute;

        }


        public bool CanExecute(object parameter) 
        {

            //   return InputString.Length >= 3;'

          //   return this.canExecute == null || this.canExecute(parameter);
            return GameModel.GameStartStatus;
        }
        

        public void Execute(object parameter)
        {
            this.execute(parameter ?? "널"); // 이거 없으면 원래 생성자 쪽에서 함수 호출 못해줌 (GameStartViewModel)

        }


        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }



      

    }
}
