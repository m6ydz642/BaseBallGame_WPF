using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Tranning
{
    /// <summary>
    /// GameStart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GameStart : Page
    {



        public int[] _randomNumberArray
        {
            get; set;
        }
        public int[] _MakeNumberSave { get; set; }

        public int[] MakeRandomNumber() // 컴퓨터 랜덤함수 생성 
        {
            // computerScore.Text = ""; // 버튼클릭시 랜덤함수 초기화

            Random rand = new Random();
            bool status = true; // 마지막에 if문으로 출력할 때 중복발견 후 출력 되야 해서
            int[] RandNum = new int[3];

            for (int i = 0; i < RandNum.Length; i++)
            {
                int number = rand.Next(1, 10); // 1부터 9까지
                status = true;
                // 중복이 발견되어 false상태이면 중복을 제외하고 출력해버리기 때문에 한번더 돌때 true로 바꿈
                for (int j = 0; j < i; j++) // 다음 인수로 중복검사하게할 값

                {
                    if (RandNum[j] == number)
                    {
                        Console.WriteLine("중복발견 : " + RandNum[j]);
                        status = false;

                        // false처리해서 배열에 값을 못넣도록 함 (밑에 array[i] = rand) 부분에 
                        i--; // 다시 반복하기 위해 -- 처리함

                        break;
                    }

                }
                if (status) // true일 경우만 출력하고 배열에 넣음
                {
                    RandNum[i] = number;
                    /* RandNum[0] = 1;
                     RandNum[1] = 2;
                     RandNum[2] = 3;*/

                }
            }
            _MakeNumberSave = RandNum;
            _randomNumberArray = RandNum;

            return _MakeNumberSave;
        }

        public GameStart()
        {

            InitializeComponent();
            
            _randomNumberArray = MakeRandomNumber();
       
            randomnumber.Content = string.Join(string.Empty, _randomNumberArray); // 랜덤함수 프로그램 실행시  화면에 바로 표시
            // int array to string 변환 
            // 화면표시용

            
            // 게임중에는 버튼실행이나 생성자 호출 안되게 수정하던지 해야 됨 
            _MakeNumberSave = _randomNumberArray;
        
        }

        private void valuebutton_Click(object sender, RoutedEventArgs e)
        {
            var row = clmAdd.ToString();
            MessageBox.Show("row : " + row);
        }
    }
}
