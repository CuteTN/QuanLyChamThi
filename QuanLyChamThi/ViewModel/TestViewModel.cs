using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class TestViewModel : ViewModelBase, UserModelBase
    {
        #region Temporary Data
        private ObservableCollection<TestModel.TestDetailModel> _tempTestDetail;
        public ObservableCollection<TestModel.TestDetailModel> TempTestDetail
        {
            get
            {
                if (_tempTestDetail == null)
                    _tempTestDetail = new ObservableCollection<TestModel.TestDetailModel>();
                return _tempTestDetail;
            }
            set { _tempTestDetail = value; OnPropertyChange("TempTestDetail"); }
        }

        private TestModel _tempTest;
        public TestModel TempTest
        {
            get
            {
                if (_tempTest == null)
                {
                    _tempTest = new TestModel();
                }
                return _tempTest;
            }
            set { _tempTest = value;
                OnPropertyChange("TempTest");
                OnPropertyChange("TempYear");
                OnPropertyChange("TempSemester");
                OnPropertyChange("TempTestID");
                OnPropertyChange("TempSubjectID");
            }
        }
        public string TempSubjectID
        {
            get { return TempTest.SubjectID; }
            set { TempTest.SubjectID = value; OnPropertyChange("TempSubjectID"); }
        }
        public string TempTestID
        {
            get { return TempTest.TestID; }
            // FOR VIEW ACCESS ONLY
            set { TempTest.TestID = value;
                ViewMode(value);
                OnPropertyChange("TempTestID"); }
        }
        public int? TempSemester
        {
            get { return TempTest.Semester; }
            set { TempTest.Semester = value; OnPropertyChange("TempSemester"); }
        }
        public int? TempYear
        {
            get { return TempTest.Year; }
            set { TempTest.Year = value; OnPropertyChange("TempYear"); }
        }
        TestModel.TestDetailModel _selectedItem;
        public TestModel.TestDetailModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChange("SelectedItem"); }
        }
        #endregion

        #region Database data
        public ObservableCollection<TestModel.TestDetailModel> TestDetail;
        private TestModel _test;
        public TestModel Test
        {
            set
            {
                _test = value;
                LoadTestDetailData();
            }
        }
        void LoadTestDetailData()
        {
            string testID = _test?.TestID;
            TestDetail = new ObservableCollection<TestModel.TestDetailModel>
                    ((from u in (from u in DataProvider.Ins.DB.TESTDETAIL
                                 where u.IDTest == testID
                                 select u)
                      join v in DataProvider.Ins.DB.QUESTION on u.IDQuestion equals v.IDQuestion
                      select new TestModel.TestDetailModel()
                      {
                          Content = v.Content,
                          QuestionID = u.IDQuestion,
                          Stt = u.No,
                          pSource = u
                      }).OrderBy((item) => item.Stt).ToList());
        }

        private List<string> _subjectID;
        public List<string> SubjectID
        {
            get
            {
                if (_subjectID == null)
                    LoadSubjectID();
                return _subjectID;
            }
            set { _subjectID = value; OnPropertyChange("SubjectID"); }
        }
        void LoadSubjectID()
        { _subjectID = (from u in DataProvider.Ins.DB.SUBJECT select u.IDSubject).ToList(); }

        private List<string> _testID;
        public List<string> TestID
        {
            get
            {
                if (_testID == null)
                    LoadTestID();
                return _testID;
            }
            set { _testID = value; OnPropertyChange("TestID"); }
        }
        void LoadTestID()
        {
            _testID = (from u in DataProvider.Ins.DB.TEST select u.IDTest).ToList();
            _testID.Add((string)(new TestModel().TestID).Clone());
        }
        #endregion

        #region View Mode: Edit or New
        public void ViewMode(string TestID)
        {
            TestModel temp = (from u in DataProvider.Ins.DB.TEST
                              where u.IDTest == TestID
                              select new TestModel()
                              {
                                  Duration = u.TimeForTest.Value,
                                  Semester = u.Semester,
                                  SubjectID = u.IDSubject,
                                  TestDate = u.DateOfTest,
                                  TestID = u.IDTest,
                                  Year = u.Year,
                                  pSource = u
                              }).FirstOrDefault();
            Test = temp;
            SyncToView();
        }

        public void SyncToView()
        {
            TempTestDetail = new ObservableCollection<TestModel.TestDetailModel>(TestDetail);
            TempTest = _test?.Clone;
        }
        #endregion

        #region Button Accept
        private ICommand _acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(param => AcceptButton());
                return _acceptCommand;
            }
            set
            {
                _acceptCommand = value;
            }
        }
        bool Validate()
        {
            if (!TempTest.Valid())
            {
                MessageBox.Show("Đề thi không hợp lệ.");
                return false;
            }
            foreach (var item in TempTestDetail)
            {
                if (item.QuestionID == 0)
                {
                    // This is not supposed to happen
                    // Just check for BS that might be thrown this way
                    MessageBox.Show("Không nhận được ID câu hỏi");
                    return false;
                }
            }
            return true;
        }
        void AcceptButton()
        {
            if (!Validate())
                return;
            List<DatabaseCommand> cmdList = new List<DatabaseCommand>();
            DatabaseCommand cmd = new DatabaseCommand();

            cmd.add = TempTest.pSource;
            cmd.delete = _test?.pSource;
            if (cmd.add != cmd.delete)
                cmdList.Add(cmd);

            for (int i = 0; true; i++)
            {
                if (i >= TempTestDetail.Count && i >= TestDetail.Count)
                    break;
                cmd = new DatabaseCommand();
                if (i < TempTestDetail.Count)
                    cmd.add = new TESTDETAIL{
                        IDQuestion = TempTestDetail[i].QuestionID,
                        IDTest = TempTest.TestID,
                        No = TempTestDetail[i].Stt
                    };
                else
                    cmd.add = null;
                if (i < TestDetail.Count)
                    cmd.delete = TestDetail[i].pSource;
                else
                    cmd.delete = null;
                // This check shouldn't be needed
                if (cmd.add != null || cmd.delete != null)
                    cmdList.Add(cmd);
                //*
                else
                    MessageBox.Show("cmd is all null.");
                //*/
            }

            if (cmdList.Any())
                ViewModelMediator.Ins.Receive(this, cmdList);
            MainWindowViewModel.Ins.SwitchView(10);
        }
        #endregion

        #region Button Cancel
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(param => CancelButton());
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
            }
        }
        void CancelButton()
        {
            SyncToView();
            MainWindowViewModel.Ins.SwitchView(10);
        }
        #endregion

        #region Button MoveUp
        private ICommand _upCommand;
        public ICommand UpCommand
        {
            get
            {
                if (_upCommand == null)
                    _upCommand = new RelayCommand(param => UpButton());
                return _upCommand;
            }
            set
            {
                _upCommand = value;
            }
        }
        private ICommand _upAllCommand;
        public ICommand UpAllCommand
        {
            get
            {
                if (_upAllCommand == null)
                    _upAllCommand = new RelayCommand(param => UpAllButton());
                return _upAllCommand;
            }
            set
            {
                _upAllCommand = value;
            }
        }
        public void UpAllButton()
        {
            int n = (SelectedItem?.Stt??1) - 1;
            if (n == 0) return;
            for (int i = n; i > 0; i--)
            //*
            {
                var temp = TempTestDetail[i];
                TempTestDetail[i] = TempTestDetail[i - 1];
                TempTestDetail[i - 1] = temp;

                TempTestDetail[i].Stt = i + 1;
                TempTestDetail[i - 1].Stt = i;
            }
            /*/
                UpButton();
            //*/
            SelectedItem = TempTestDetail[0];
        }
        public void UpButton()
        {
            int i = (SelectedItem?.Stt??1) - 1;
            if (i == 0)
                return;

            var temp = TempTestDetail[i];
            TempTestDetail[i] = TempTestDetail[i - 1];
            TempTestDetail[i - 1] = temp;

            TempTestDetail[i].Stt = i + 1;
            TempTestDetail[i - 1].Stt = i;

            SelectedItem = TempTestDetail[i - 1];
        }
        #endregion

        #region Button MoveDown

        private ICommand _downCommand;
        public ICommand DownCommand
        {
            get
            {
                if (_downCommand == null)
                    _downCommand = new RelayCommand(param => DownButton());
                return _downCommand;
            }
            set
            {
                _downCommand = value;
            }
        }
        private ICommand _downAllCommand;
        public ICommand DownAllCommand
        {
            get
            {
                if (_downAllCommand == null)
                    _downAllCommand = new RelayCommand(param => DownAllButton());
                return _downAllCommand;
            }
            set
            {
                _downAllCommand = value;
            }
        }
        public void DownAllButton()
        {
            int n = (SelectedItem?.Stt ?? 1);
            int m = TempTestDetail.Count;
            if (m == 0) return;
            for (int i = n; i < m; i++)
            //*
            {
                var temp = TempTestDetail[i];
                TempTestDetail[i] = TempTestDetail[i - 1];
                TempTestDetail[i - 1] = temp;

                TempTestDetail[i].Stt = i + 1;
                TempTestDetail[i - 1].Stt = i;
            }
            /*/
                DownButton();
            //*/
            SelectedItem = TempTestDetail[m - 1];
        }
        public void DownButton()
        {
            int i = SelectedItem?.Stt ?? -10;
            if (i == TempTestDetail.Count || i < 0)
                return;

            var temp = TempTestDetail[i];
            TempTestDetail[i] = TempTestDetail[i - 1];
            TempTestDetail[i - 1] = temp;

            TempTestDetail[i].Stt = i + 1;
            TempTestDetail[i - 1].Stt = i;

            SelectedItem = TempTestDetail[i];
        }
        #endregion

        #region Basic View Model info
        public TestViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }

        public void AcceptData(List<QuestionModel> questions)
        {
            TempTestDetail.Clear();
            TempTestDetail = new ObservableCollection<TestModel.TestDetailModel>(
                questions.Select((QuestionModel question, int i) => new TestModel.TestDetailModel(question, i + 1)));
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            DatabaseCommand test = commands.FirstOrDefault((DatabaseCommand item) => item.delete != null && (item.delete is TEST) && item.delete == _test?.pSource);
            if (test != null)
            {
                ViewMode((test.add as TEST)?.IDTest);
                // ASSERT delete only once
                return;
            }
            string testID = _test?.TestID;
            if (commands.Any((DatabaseCommand item) 
                => (item.add is TESTDETAIL && (item.add as TESTDETAIL).IDTest == testID)
                || (item.delete is TESTDETAIL && (item.delete as TESTDETAIL).IDTest == testID)))
                    LoadTestDetailData();
            if (commands.Any((DatabaseCommand item) => item.add is SUBJECT || item.delete is SUBJECT))
            {
                LoadTestDetailData();
                LoadSubjectID();
            }
            if (commands.Any((DatabaseCommand item) => item.add is TEST || item.delete is TEST))
            {
                LoadTestID();
            }

        }
        #endregion
    }
}
