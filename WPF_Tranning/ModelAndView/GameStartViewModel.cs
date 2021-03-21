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
        public ICommand Retry { get; set; } // 재시작 버튼

        public ICommand Enter { get; set; } // 키패드 버튼 입력 후 게임 시작 버튼

        public bool StatusGateStart { set; get; } // 게임시작여부

 

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
            this.Retry = new Retry(this);
            this.Enter = new EnterGame(this);
            this.CheckBinding  = new RelayCommand(new Action<object>(this.CheckBoxs)); 


            //   Enter = new RelayCommand(new Action<object>(Enter.Execute));

            _datatable = new DataTable();
            _datatable.Columns.Add("count");
            _datatable.Columns.Add("사용자 입력");
            _datatable.Columns.Add("점수");

            /* _selecttable = new DataTable(); // 임시로 줄추가 
             _selecttable.Columns.Add("호우"); // 임시로 추가
             _selecttable.Rows.Add("와우"); // 임시로 추가*/

            //    DataSet dataSet = connectDB();

            _selecttable = connectDB().Tables[0]; // select한 값 넣음


        }

        private void CheckBoxs(object a)
        {
            MessageBox.Show("체크!");
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
                //    MessageBox.Show("프로퍼티 체인지!!!");
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }


    }

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
            MessageBox.Show("클릭 Execute");
            this.execute(parameter ?? "널이얌"); // 앙 파라메터띠
        }


    }
}
