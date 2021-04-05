using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Tranning
{
    /// <summary>
    /// GameStart.xaml에 대한 상호 작용 논리
    /// </summary>
    public  partial class GameStart : Page
    {



       

        public GameStart()
        {

            InitializeComponent();

         //   randomnumber.Content = string.Join(string.Empty, _randomNumberArray); // 랜덤함수 프로그램 실행시  화면에 바로 표시
                                                                                  // int array to string 변환 
                                                                                  // 화면표시용


            // 게임중에는 버튼실행이나 생성자 호출 안되게 수정하던지 해야 됨 
            // _MakeNumberSave = _randomNumberArray;

        }

        private void valuebutton_Click(object sender, RoutedEventArgs e)
        {
            var row = clmAdd.ToString();
       
        }
    }
}
