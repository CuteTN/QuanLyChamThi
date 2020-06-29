using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.Windows.Input;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Windows;
using QuanLyChamThi.View;
using System.Windows.Forms;

namespace QuanLyChamThi.ViewModel
{
    class TestResultViewModel: ViewModelBase, UserModelBase
    {
        TESTRESULTDETAIL oldTestResultDetail;

        bool _allSelected;
        public bool AllSelected
        {
            get { return _allSelected; }
            set { _allSelected = value; }
        }

        #region Combobox Subject
        private BindingList<SUBJECT> _listSubject;
        public BindingList<SUBJECT> ListSubject
        {
            get 
            {
                if (_listSubject == null)
                    _listSubject = new BindingList<SUBJECT>((from u in DataProvider.Ins.DB.SUBJECT select u).ToList());
                return _listSubject; 
            }
            set { _listSubject = value; OnPropertyChange("ListSubject"); }
        }

        private SUBJECT _selectedSubject;
        public SUBJECT SelectedSubject
        {
            get
            {
                if(_selectedClass != null)
                {
                    _selectedSubject = (from u in DataProvider.Ins.DB.SUBJECT
                                        where u.IDSubject == _selectedClass.Subject.IDSubject
                                        select u).ToList().First();
                }
                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                OnPropertyChange("SelectedSubject");
                ListClass = null;
                ListTestID = null;
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Combobox Class

        private BindingList<ClassModel> _listClass;
        public BindingList<ClassModel> ListClass
        {
            get
            {
                if (_listClass == null)
                {
                    if (_selectedSubject != null)
                    {
                        _listClass = new BindingList<ClassModel>((from c in DataProvider.Ins.DB.CLASS
                                                                  join s in DataProvider.Ins.DB.SUBJECT on c.IDSubject equals s.IDSubject
                                                                  where c.IDSubject == _selectedSubject.IDSubject
                                                                  select new ClassModel
                                                                  {
                                                                      IDClass = c.IDClass,
                                                                      Subject = s,
                                                                      ClassName = c.Name,
                                                                      Year = c.Year,
                                                                      Semester = c.Semester,
                                                                      Username = c.Username
                                                                  }).ToList());
                    }
                    else
                    {
                        _listClass = new BindingList<ClassModel>((from c in DataProvider.Ins.DB.CLASS
                                                                  join s in DataProvider.Ins.DB.SUBJECT on c.IDSubject equals s.IDSubject
                                                                  select new ClassModel
                                                                  {
                                                                      IDClass = c.IDClass,
                                                                      Subject = s,
                                                                      ClassName = c.Name,
                                                                      Year = c.Year,
                                                                      Semester = c.Semester,
                                                                      Username = c.Username
                                                                  }).ToList());
                    }
                }
                return _listClass;
            }
            set { _listClass = value; OnPropertyChange("ListClass"); }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get
            {
                return _selectedClass;
            }
            set
            {
                _selectedClass = value;
                OnPropertyChange("SelectedClass");
                OnPropertyChange("SelectedSubject");
                OnPropertyChange("ListTestID");
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Combobox IDTest

        private BindingList<TEST> _listTestID;
        public BindingList<TEST> ListTestID
        {
            get
            {
                if (_listTestID == null)
                {
                    if (_selectedSubject != null)
                    {
                        var x = DataProvider.Ins.DB;
                        _listTestID = new BindingList<TEST>((from u in DataProvider.Ins.DB.TEST
                                                             where u.IDSubject == _selectedSubject.IDSubject
                                                             select u).ToList());
                    }
                    else
                    {
                        _listTestID = new BindingList<TEST>(DataProvider.Ins.DB.TEST.ToList());
                    }
                }
                return _listTestID;
            }
            set
            {
                _listTestID = value;
                OnPropertyChange("ListTestID");
            }
        }

        private TEST _selectedTestID;
        public TEST SelectedTestID
        {
            get
            {
                return _selectedTestID;
            }
            set
            {
                _selectedTestID = value;
                OnPropertyChange("SelectedTestID");
                (SaveCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region DataGrid ListTestResult
        private ObservableCollection<TestResultModel> _listTestResult;
        public ObservableCollection<TestResultModel> ListTestResult
        {
            get
            {
                if(_listTestResult == null)
                {
                    _listTestResult = new ObservableCollection<TestResultModel>();
                }
                return _listTestResult;
            }
            set
            {
                _listTestResult = value;
                OnPropertyChange("ListTestResult");
            }
        }

        ICommand _selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                if (_selectAllCommand == null)
                    _selectAllCommand = new RelayCommand(param => SelectAll());
                return _selectAllCommand;
            }
            set { _selectAllCommand = value; }
        }
        public void SelectAll()
        {
            foreach(var testResult in ListTestResult)
            {
                testResult.Selected = AllSelected;
                OnPropertyChange("testResult.Selected");
            }
        }

        ICommand _deleteSelectedResultsCommand;
        public ICommand DeleteSelectedResultsCommand
        {
            get
            {
                if(_deleteSelectedResultsCommand == null)
                {
                    _deleteSelectedResultsCommand = new RelayCommand(param => DeleteSelectedResults());
                }
                return _deleteSelectedResultsCommand;
            }
            set { _deleteSelectedResultsCommand = value; }
        }
        public void DeleteSelectedResults()
        {
            for(int i=0; i<ListTestResult.Count;)
            {
                if (ListTestResult[i].Selected)
                    ListTestResult.RemoveAt(i);
                else
                    i++;
            }
        }

        ICommand _updateAllCheckCommand;
        public ICommand UpdateAllCheckCommand
        {
            get
            {
                if (_updateAllCheckCommand == null)
                    _updateAllCheckCommand = new RelayCommand(param => UpdateAllCheck());
                return _updateAllCheckCommand;
            }
            set { _updateAllCheckCommand = value; }
        }

        void UpdateAllCheck()
        {
            AllSelected = true;
            foreach(var testResult in ListTestResult)
            {
                AllSelected = AllSelected & testResult.Selected;
            }
            OnPropertyChange("AllSelected");
        }

        #endregion

        #region Button Add
        private ICommand _addNewTestResultCommand;
        public ICommand AddNewTestResultCommand
        {
            get 
            {
                if (_addNewTestResultCommand == null)
                    _addNewTestResultCommand = new RelayCommand(param => AddNewTestResult());
                return _addNewTestResultCommand;
            }
            set
            {
                _addNewTestResultCommand = value;
            }
        }

        void AddNewTestResult()
        {
            ListTestResult.Add(new TestResultModel());
        }

        #endregion

        #region Button Save

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => Save(), param => CanSave());
                return _saveCommand;
            }
            set { _saveCommand = value; }
        }

        void Save()
        {
            if (ValidateDuplicatedTestResult() == false)
                return;

            foreach (var testResult in _listTestResult)
            {
                if (!ValidTestResult(testResult))
                    return;
            }

            // send command for save new TESTRESULT to database via ViewModelMediator
            try
            {
                var studentCommands = GenerateStudentCommands();
                var changedCommands = GenerateChangeCommands();
                ViewModelMediator.Ins.Receive(this, studentCommands);
                ViewModelMediator.Ins.Receive(this, changedCommands);
            }
            catch(Exception e)
            {
                if(e.InnerException != null)
                {
                    ViewExtension.MessageOK(null, "Lỗi: " + e.InnerException.Message, ViewExtension.MessageType.Error);
                }
                else
                {
                    ViewExtension.MessageOK(null, "Lỗi: Không xác định.\n Error code: 0x0001", ViewExtension.MessageType.Error);
                }

            }

            MainWindowViewModel.Ins.SwitchView(8);
        }

        
        string GenerateIDTestResultForAllResults()
        {
            string IDTestResult;
            if (oldTestResultDetail == null)
                IDTestResult = DateTime.Now.ToString();
            else
                IDTestResult = oldTestResultDetail.IDTestResult;
            foreach (var testResult in _listTestResult)
            {
                testResult.IDTestResult = IDTestResult;
            }
            return IDTestResult;
        }

        #region Ganerate DBCommands
        // Auto generate student in db if student is new
        List<DatabaseCommand> GenerateStudentCommands()
        {
            List<DatabaseCommand> commands = new List<DatabaseCommand>();

            foreach (var testResult in _listTestResult)
            {
                if (DataProvider.Ins.DB.STUDENT.Where(param => param.IDStudent == testResult.StudentID).ToList().Count == 0)
                {
                    commands.Add(new DatabaseCommand
                    {
                        add = new STUDENT { IDStudent = testResult.StudentID, FullName = testResult.StudentName },
                        delete = null
                    });
                }
            }
            return commands;
        }

        List<DatabaseCommand> GenerateChangeCommands()
        {
            string IDTestResult = GenerateIDTestResultForAllResults();

            List<DatabaseCommand> commands = new List<DatabaseCommand>();

            commands.Add(new DatabaseCommand
            {
                add = new TESTRESULTDETAIL
                {
                    IDTestResult = IDTestResult,
                    IDClass = _selectedClass.IDClass,
                    IDTest = _selectedTestID.IDTest,
                    Username = "01",
                    Note = ""
                },
                delete = oldTestResultDetail
            });

            if(oldTestResultDetail == null)
            {
                foreach (var testResult in _listTestResult)
                {
                    var add = new TESTRESULT
                    {
                        IDTestResult = testResult.IDTestResult,
                        IDStudent = testResult.StudentID,
                        ScoreNumber = testResult.ScoreNumber,
                        ScoreString = testResult.ScoreString,
                        Note = testResult.Note
                    };
                    commands.Add(new DatabaseCommand
                    {
                        add = add,
                        delete = null
                    });
                }
                return commands;
            }

            List<TESTRESULT> dbTestResults = DataProvider.Ins.DB.TESTRESULTDETAIL.Find(IDTestResult).TESTRESULT.ToList();
            foreach (var testResult in _listTestResult)
            {
                var add = new TESTRESULT
                {
                    IDTestResult = testResult.IDTestResult,
                    IDStudent = testResult.StudentID,
                    ScoreNumber = testResult.ScoreNumber,
                    ScoreString = testResult.ScoreString,
                    Note = testResult.Note
                };
                var existingTestResult = dbTestResults.Find(param => param.IDTestResult == add.IDTestResult &&
                                                            param.IDStudent == add.IDStudent);
                // if new this testResult doesnot appear in the current db, add it
                if (existingTestResult == null)
                {
                    commands.Add(new DatabaseCommand
                    {
                        add = add,
                        delete = null
                    });
                }
                // if it exist in db, check if it modified
                else if (add.ScoreNumber != existingTestResult.ScoreNumber ||
                         add.ScoreString != existingTestResult.ScoreString ||
                         add.Note        != existingTestResult.Note)
                {
                    commands.Add(new DatabaseCommand
                    {
                        add = add,
                        delete = existingTestResult
                    });
                }
            }

            foreach(var testResult in dbTestResults)
            {
                // if there is any testResult in db but not appear in current _listTestResult, that mean it has been deleted
                if(_listTestResult.Where(param => (param.IDTestResult == testResult.IDTestResult &&
                                                  param.StudentID == testResult.IDStudent)).ToList().Count == 0)
                {
                    commands.Add(new DatabaseCommand
                    {
                        add = null,
                        delete = testResult
                    });
                }
            }
            return commands;
        }
        #endregion

        bool CanSave()
        {
            if (_selectedClass == null || _selectedSubject == null || _selectedTestID == null)
                return false;

            return true;
        }

        #region Validate Funtion
        bool ValidTestResult(TestResultModel testResult)
        {
            STUDENT student = DataProvider.Ins.DB.STUDENT.Find(testResult.StudentID);
            if (student != null && student.FullName != testResult.StudentName)
            {
                // Warn user that student with given id have wrong name // TODO
                ViewExtension.MessageOK(null, "Lỗi: Học sinh với mssv " + student.IDStudent + " bị sai tên\n Tên đúng là " + student.FullName, ViewExtension.MessageType.Error);
                //////////////////////////////////////////////////////////
                return false;
            }
            if(testResult.ScoreNumber < DataProvider.Ins.DB.PRINCIPLE.ToList()[0].MinScore ||
               testResult.ScoreNumber > DataProvider.Ins.DB.PRINCIPLE.ToList()[0].MaxScore)
            {
                // Notify user that the score is not in principle // TODO
                ViewExtension.MessageOK(null, "Lỗi: Học sinh với mssv " + student.IDStudent + " có điểm không nằm trong khoảng quy định\n Khoảng quy định: " + DataProvider.Ins.DB.PRINCIPLE.First().MinScore + "...." + DataProvider.Ins.DB.PRINCIPLE.First().MaxScore, ViewExtension.MessageType.Error);
                ////////////////////////////////////////////////////
                return false;
            }
            return true;
        }
        #endregion

        bool ValidateDuplicatedTestResult()
        {
            foreach(var testResult in ListTestResult)
            {
                if(ListTestResult.Where(param => param.StudentID == testResult.StudentID).ToList().Count > 1)
                {
                    ViewExtension.MessageOK(null, "Lỗi: Không thể tồn tại 2 bài chấm với cùng 1 MSSV\n" + "MSSV bị trùng: " + testResult.StudentID, ViewExtension.MessageType.Error);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Button Cancel
        ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(param => Cancel());
                return _cancelCommand;
            }
            set { _cancelCommand = value; }
        }
        void Cancel()
        {
            if (ViewExtension.Confirm(null, "Bạn có chắc muốn hủy các thay đổi không?") == 0)
                return;
            MainWindowViewModel.Ins.SwitchView(8);
        }
        #endregion
        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // We handle command that received from ViewModelMediator here
            resetEditing();
        }

        public void resetEditing()
        {
            ListSubject = null;
            ListClass = null;
            ListTestID = null;
            ListTestResult = null;

            SelectedClass = null;
            SelectedSubject = null;
            SelectedTestID = null;
        }

        public void StartEditing(object arg)
        {
            oldTestResultDetail = arg as TESTRESULTDETAIL;
            if (arg == null)
            {
                resetEditing();
            }
            else
            {
                resetEditing();


                if (oldTestResultDetail.IDClass != null || oldTestResultDetail.IDTest != null)
                {
                    SelectedSubject = (from c in DataProvider.Ins.DB.CLASS
                                       join t in DataProvider.Ins.DB.TEST on c.IDSubject equals t.IDSubject
                                       where c.IDClass == oldTestResultDetail.IDClass || t.IDTest == oldTestResultDetail.IDTest
                                       join s in DataProvider.Ins.DB.SUBJECT on c.IDSubject equals s.IDSubject
                                       select s).ToList()[0];

                    SelectedClass = ListClass.FirstOrDefault(param => param.IDClass == oldTestResultDetail.IDClass);
                }

                SelectedTestID = DataProvider.Ins.DB.TEST.Find(oldTestResultDetail.IDTest);
                if (!ListTestID.Contains(SelectedTestID))
                {
                    ListTestID.Add(SelectedTestID);
                    OnPropertyChange("SelectedTestID");
                }

                ListTestResult = new ObservableCollection<TestResultModel>(from t in DataProvider.Ins.DB.TESTRESULT
                                                                            join s in DataProvider.Ins.DB.STUDENT on t.IDStudent equals s.IDStudent
                                                                            where t.IDTestResult == oldTestResultDetail.IDTestResult
                                                                            select new TestResultModel
                                                                            {
                                                                                Selected = false,
                                                                                IDTestResult = t.IDTestResult,
                                                                                StudentID = s.IDStudent,
                                                                                StudentName = s.FullName,
                                                                                ScoreNumber = (float)t.ScoreNumber,
                                                                                ScoreString = t.ScoreString,
                                                                                Note = t.Note
                                                                            });
            }
        }

        public TestResultViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
        
    }
}
