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
                    _tempTest.TestID = "Bài thi mới";
                }
                return _tempTest;
            }
            set { _tempTest = value; OnPropertyChange("TempTest"); }
        }
        public string TempSubjectID
        {
            get { return TempTest.SubjectID; }
            set { TempTest.SubjectID = value; OnPropertyChange("TempSubjectID"); ; }
        }
        public string TempTestID
        {
            get { return TempTest.TestID; }
            set { TempTest.TestID = value; OnPropertyChange("TempTestID"); ; }
        }
        public int? TempSemester
        {
            get { return TempTest.Semester; }
            set { TempTest.Semester = value; OnPropertyChange("TempSemester"); ; }
        }
        public int? TempYear
        {
            get { return TempTest.Year; }
            set { TempTest.Year = value; OnPropertyChange("TempYear"); ; }
        }
        #endregion

        #region Binded Data
        public ObservableCollection<TestModel.TestDetailModel> TestDetail;
        private TestModel _test;
        public TestModel Test
        {
            set
            {
                _test = value;
                string testID = _test?.TestID;
                TestDetail = new ObservableCollection<TestModel.TestDetailModel>
                        ((from u in (from u in DataProvider.Ins.DB.TESTDETAIL
                                     where u.IDTest == testID select u)
                          join v in DataProvider.Ins.DB.QUESTION on u.IDQuestion equals v.IDQuestion
                          select new TestModel.TestDetailModel()
                          {
                                Content = v.Content,
                                QuestionID = u.IDQuestion,
                                Stt = u.No,
                                pSource = u
                          }).OrderBy((item)=>item.Stt).ToList());
            }
        }
        private List<string> _subjectID;
        public List<string> SubjectID
        {
            get
            {
                if (_subjectID == null)
                    _subjectID = (from u in DataProvider.Ins.DB.SUBJECT select u.IDSubject).ToList();
                return _subjectID;
            }
            set { _subjectID = value; }
        }
        #endregion

        #region View Mode: Edit or New
        public void ViewMode(string TestID)
        {
            TestModel temp = (from u in DataProvider.Ins.DB.TEST
                              where u.IDTest == TestID
                              select new TestModel()
                              {
                                  Duration = u.TimeForTest,
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
        void AcceptButton()
        {
            List<DatabaseCommand> cmdList = new List<DatabaseCommand>();
            DatabaseCommand cmd = new DatabaseCommand();

            cmd.add = TempTest.pSource;
            cmd.delete = _test?.pSource;
            cmdList.Add(cmd);
            
            for (int i=0; true; i++)
            {
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
                cmdList.Add(cmd);
                if (i >= TempTestDetail.Count && i >= TestDetail.Count)
                    break;
            }
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
        #endregion

        #region Button MoveDown
        #endregion

        #region Basic View Model info
        public TestViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            DatabaseCommand test = commands.FirstOrDefault((DatabaseCommand item) => item.delete != null && item.delete == _test?.pSource);
            if (test != null)
            {
                ViewMode((test.add as TEST)?.IDTest);
                // ASSERT delete only once
                return;
            }
            string testID = _test?.TestID;
            if (commands.Any((DatabaseCommand item) => item.add is SUBJECT || item.delete is SUBJECT
            || (item.add is TESTDETAIL && (item.add as TESTDETAIL).IDTest == testID)
            || (item.delete is TESTDETAIL && (item.delete as TESTDETAIL).IDTest == testID)))
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
                          }).OrderBy((item)=>item.Stt).ToList());
        }
        #endregion
    }
}
