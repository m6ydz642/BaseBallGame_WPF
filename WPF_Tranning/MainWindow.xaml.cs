using BaseBallGame_WPF.ModelAndView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Shapes;

namespace WPF_Tranning
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new GameStartViewModel(); // 바인딩 설정 
            // GameStartViewModel 호출인줄 알았는데 바인딩 설정을 저 클래스로 함

        }

        private void btnmain_Click(object sender, RoutedEventArgs e)
        {
            // nav_content.Source = new Uri("View/Introduce.xaml", UriKind.Relative); // uri로 페이지 이동
           // Introduce a = new Introduce();
   
        //   nav_content.Source = new Introduce();
           nav_content.Source = new Introduce(); // UserControl만 이 방식으로 됨 ㅡ.ㅡ
            // page, window 안됨

          
        }

        private void btnstart_Click(object sender, RoutedEventArgs e)
        {
            nav_content.Source = new Uri("View/GameStart.xaml", UriKind.Relative); // uri로 페이지 이동
        }
    }
}
