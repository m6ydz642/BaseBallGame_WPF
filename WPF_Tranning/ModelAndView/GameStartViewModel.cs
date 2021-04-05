using BaseBallGame_WPF;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

using System.Windows.Input;
using WPF_Tranning;

namespace WPF_Tranning
{

    public class GameStartViewModel : INotifyPropertyChanged
    {

        public ICommand ClickKeyPad { get; set; } // 1~9까지 버튼 키패드 클릭
        public ICommand BackSpaceCommand { protected set; get; } // 한칸씩 삭제 버튼

        public ICommand retry;
        public ICommand Retry {
            get {
                return retry;
            }
            set {
                retry = value;
                OnPropertyChanged("Retry"); // 이거 알려줘야 RelayCommand쪽에서 Excute들음
            }
        } // 재시작 버튼

        private ICommand _enter;
        public ICommand Enter { get; set; }

      //  public bool StatusGateStart { set; get; } // 게임시작여부

        public GameModel model;

        //View와 바인딩된 속성값이 바뀔때 이를 WPF에게 알리기 위한 이벤트
        public event PropertyChangedEventHandler PropertyChanged;

        //출력될 문자들을 담아둘 변수

        public string _inputString;

        //계산기화면의 출력 텍스트박스에 대응되는 필드
        string _displayText;

        CheckValue _checkvalue;
        public int _countGame;

        public ICommand _checkBinding;
        public ICommand CheckBinding
        {
            get
            {
                return _checkBinding;
            }
            set
            {

                _checkBinding = value;
            }
        }
        /**********************************************************************/
        public DataTable _datatable;


        public DataTable DataTable
        {
            get { return _datatable; }
            set
            {
                _datatable = value;
                MessageBox.Show("데이터 테이블");
                //   RaisePropertyChanged("DataTable");
            }
        }
        /**********************************************************************/
        string AppconfigDBSetting = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString; // DB연결
        /**********************************************************************/
        public DataTable _selecttable;


        public DataTable SelectTable
        {
            get {/* MessageBox.Show("데이터 테이블");*/ return _selecttable; }
            set
            {
                _selecttable = value;

                //   RaisePropertyChanged("DataTable");
            }
        }
        /**********************************************************************/
        /*      public string _checkBox;

              public string CheckBox
              {
                  get {*//* MessageBox.Show("앙 바인딩 : " + _checkBox) ;*//*  return _checkBox;  }
                  set
                  {
                      _checkBox = value;
                  }

              }*/

        public interface IBaseCommand : ICommand
        {
            void OnCanExecuteChanged();
        }

        public GameStartViewModel() // 기본생성자
        {

            _inputString = "";
            _displayText = "";
            _countGame = 0;
            //    this.ClickKeyPad = new RelayCommand(new Action<object>(this.KeyPadClick)); // 아직은 안쓰는데 조만간 뭐 전달한다고 쓸듯

            this.ClickKeyPad = new AddNumberKeyPad(this); // 키패드 클릭시 객체 전달
                                                          //숫자 버튼을 클릭할 때 실행                                 
            this.BackSpaceCommand = new BackSpaceCommand(this);
            this._checkvalue = new CheckValue();
            this.Retry = new Retry(new Action<object>(this.RetryCheck));
            this.Enter = new EnterGame(new Action<object>(this.EnterGame));

            // this.Enter = new EnterGame(this);
            this.CheckBinding = new RelayCommand(new Action<object>(this.CheckBoxs));


            //   Enter = new RelayCommand(new Action<object>(Enter.Execute));

            _datatable = new DataTable();
            _datatable.Columns.Add("count");
            _datatable.Columns.Add("사용자 입력");
            _datatable.Columns.Add("점수");



            _selecttable = connectDB().Tables[0]; // select한 값 넣음
            _randomNumberArray = MakeRandomNumber();
            GameModel.GameStartStatus = true; // 바로 게임 시작할 수 있게

        }

        private void RetryCheck(object obj)
        {
            MessageBox.Show("게임을 다시시작합니다 : " + obj);
            GameModel.Count = 0;
            GameModel.GameStartStatus = true;
            _datatable.Rows.Clear();
            MakeRandomNumber(); // 랜덤번호 변경
        }

        private void CheckBoxs(object a)
        {
            MessageBox.Show("체크!");
        }

        public void EnterGame(object a)
        {
            string value = InputString; // 그냥 입력받았던거 그대로 컬럼명에 보여주는 단순한 표시용도임 실제 계산은 프로그램 내에서 할거


            // checkvalue.inputNumber(value); // 입력받은 값을 배열로 만들어서 가지고 나감
            // 객체생성 생성자에서 만들어 와서 해보려고 했는데 오류나서 일단 객체 생성해봄

            inputNumber(value); // 입력받은 값을 배열로 만들어서 가지고 나감

            _datatable.Rows.Add(++GameModel.Count, value, calcScore()); // count를 static으로 전달
            InputString = ""; // 엔터치면 초기화 함

            calcScore();
          

        }
        public DataSet connectDB()
        {
            string selectQuery = ConfigurationManager.AppSettings["selectScore"];
            SqlConnection connection = new SqlConnection(AppconfigDBSetting);
            connection.Open(); // DB연결

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectQuery, connection); // DB통로
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet); // dataset으로 채움
            /*  
              dataGridView1.DataSource = dataSet.Tables[0];*/

            // DataSet 리턴받아 호출하는 곳에서 나머지 Tables등 실행함
            return dataSet;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string InputString
        {
            set
            {
                if (_inputString != value/* && _checkvalue.checkKeypadLength(InputString)*/)
                {
                    _inputString = value;

                    OnPropertyChanged("InputString"); // 값이 들어왔으면 PropertyChanged를 호출함 (변경되었다고 알림)
                                                      // ((BackSpaceCommand)this.BackSpaceCommand).OnCanExecuteChanged();
                    ((BackSpaceCommand)BackSpaceCommand).OnCanExecuteChanged();

                    // gamestartviewmodel을 backspacecommand로 형변환
                    /*   if (value != "") // 이거때문에 마지막 안지워지는거였네 ㅅㅂ ㅋㅋㅋㅋ
                       {*/
                    DisplayText = value; // 디스플레이에 전달
                                         // }

                }


            }
            get { return _inputString; }

        }


        public string DisplayText
        {
            set
            {
                if (_displayText != value) // 이거 용도가 뭐지 ㅡ.ㅡ
                {
                    _displayText = value;
                    OnPropertyChanged("DisplayText"); // 값이 들어왔으면 PropertyChanged를 호출함 (변경되었다고 알림)
                                                      // 디스플레이 표시
                    ((Retry)Retry).OnCanExecuteChanged();
                }
            }
            get { return _displayText; }

        }


        protected void OnPropertyChanged(string propertyName)

        {

            if (PropertyChanged != null)

            {
                // MessageBox.Show("프로퍼티 체인지!!!");
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }





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



        public int[] _inputNumberSave { get; set; }

        public int[] _SaveScore { get; set; }


        public int[] inputNumber(string value)
        { // 사용자 입력값
            char[] inputNumberString = value.ToCharArray();



            _inputNumberSave = new int[3]; // new 연산자로 영역 생성안하니까 null뜨면서 안들

            // char[] 타입을 int [] 타입으로
            // 이거 어이없는게 0번(48) 아스키코드를 빼면 현재 숫자가 나옴
            // 아스키코드로 49는 1인데 이걸 변환하려면 -48을 하면 1이나옴 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ

            _inputNumberSave[0] = inputNumberString[0] - 48;
            _inputNumberSave[1] = inputNumberString[1] - 48;
            _inputNumberSave[2] = inputNumberString[2] - 48;

            // setter에 동시에 넣으면서
            return _inputNumberSave; // setter값을 리턴함 비교는 컴퓨터가 만든 랜덤값과 할거임
        }



        public int[] game {get; set;}
        public string calcScore() // 게임 점수계산 함수
        {
            int Strike = 0;// 게임시작시 사용자에게 한 회차끝나고 보여지는 점수 
            int Ball = 0; // 게임시작시 사용자에게 한 회차끝나고 보여지는 점수 
            string Message = "";
            int[] RandNum = _MakeNumberSave;

            for (int i = 0; i < _inputNumberSave.Length; i++)
            {
                if (RandNum[i] == _inputNumberSave[i])
                {
                    Strike++; // 사용자에게 보여지는 점수판
                }

                for (int j = 0; j < _inputNumberSave.Length; j++)
                {
                    if (RandNum[i] == _inputNumberSave[j] && i != j)
                    {

                        Ball++;
                    }
                }
                Message = Strike + "스트라이크 " + Ball + "볼";
            }
            if(Strike >= 3)
            {
                MessageBox.Show("사용자가 승리하였습니다 게임을 종료합니다");
                GameModel.GameStartStatus = false; // 게임 강제중단처리
               
            }


            return Message;
        }


    }
}
