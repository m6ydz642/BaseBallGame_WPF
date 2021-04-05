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
    class Retry : IBaseCommand
    {
        private Action<object> execute;

        private Func<object, bool> canExecute;
      

    

        // OnCanExecuteChanged 메소드의
        // ommandManager.InvalidateRequerySuggested()를 호출하면
        // CanExecuteChanged 이벤트가 호출되어
        // CanExecute로 해당 Command가 있는 버튼을 활성화 또는 비활성화

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public Retry()
        {
          
        }
        public Retry(Action<object> execute, Func<object, bool> canExecute = null)

        {

            this.execute = execute;

            this.canExecute = canExecute;

        }
        public bool CanExecute(object parameter)
        {
            // MessageBox.Show("canExcute 호출");
            //  return viewModel.StatusGateStart; // 일단 게임 초반은 false상태로 시작하기때문에 시작하자마자 재시작 버튼은 비활성화 상태임 
            // return true;
            int count = GameModel.Count;
            return count >= 1 || GameModel.GameStartStatus == false;
        }

  

        public void Execute(object parameter)
        {
            MessageBox.Show("retry Excute 호출 : " + GameModel.Count);
            this.execute(parameter ?? "널"); // 이거 없으면 원래 생성자 쪽에서 함수 호출 못해줌 (GameStartViewModel)

        }


        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
