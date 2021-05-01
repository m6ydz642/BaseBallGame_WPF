using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BaseBallGame_WPF.ModelAndView
{
    class IntroducePageBinding 
    {
        public ICommand Test { get; set; }

        public IntroducePageBinding()
        {
            Test = new RelayCommand(new Action<object>(this.TestFunction));
         //  MessageBox.Show("IntroducePageBinding 생성자");
        }

        private void TestFunction(object obj)
        {
            MessageBox.Show("네비게이션 프레임에서 바인딩 테스트");
        }
    }
}
