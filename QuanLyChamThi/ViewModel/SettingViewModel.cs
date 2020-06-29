using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using QuanLyChamThi.View;

namespace QuanLyChamThi.ViewModel
{
    class SettingViewModel : ViewModelBase, UserModelBase
    {
        // THANHCODE

        #region Difficulty Setting

        #region Textbox Số lượng độ khó
        private int? _numberOfDifficulty;
        public int? NumberOfDifficulty
        {
            get
            {
                if (_numberOfDifficulty == null)
                    _numberOfDifficulty = ListDifficulty.Count;
                return _numberOfDifficulty;
            }
            set { _numberOfDifficulty = value; OnPropertyChange("NumberOfDifficulty"); }
        }
        #endregion

        #region Button Cập nhật
        private ICommand _updateNumberDifficultyCommand;
        public ICommand UpdateNumberDifficultyCommand
        {
            get
            {
                if (_updateNumberDifficultyCommand == null)
                    _updateNumberDifficultyCommand = new RelayCommand(param => UpdateNumberDifficulty());
                return _updateNumberDifficultyCommand;
            }
            set
            {
                _updateNumberDifficultyCommand = value;
            }
        }
        private void UpdateNumberDifficulty()
        {
            for (int i = ListDifficulty.Count; i != NumberOfDifficulty;)
            {
                if (ListDifficulty.Count < NumberOfDifficulty)
                {
                    ListDifficulty.Add(new DifficultyModel(new Random().Next(), ""));
                    i++;
                }
                else
                {
                    ListDifficulty.RemoveAt(i - 1);
                    i--;
                }
            }
            OnPropertyChange("ListDifficulty");
        }
        #endregion

        #region Button OK

        

        void SaveChangeDifficulty()
        {
            List<DatabaseCommand> commands = GenerateCommandsDifficulty();
            DisableDeletedQuestion();
            ViewModelMediator.Ins.Receive(this, commands);
        }

        List<DatabaseCommand> GenerateCommandsDifficulty()
        {
            List<DatabaseCommand> commands = new List<DatabaseCommand>();

            foreach (var difficulty in ListDifficulty)
            {
                var difficultyInDB = DataProvider.Ins.DB.DIFFICULTY.Where(param => param.IDDifficulty == difficulty.IDDifficulty).ToList();
                DIFFICULTY deleteDifficulty = difficultyInDB.Count() == 0 ? null : difficultyInDB[0];
                DIFFICULTY addDifficulty = new DIFFICULTY
                {
                    IDDifficulty = difficulty.IDDifficulty,
                    Name = difficulty.Name,
                    Disabled = false
                };
                commands.Add(new DatabaseCommand
                {
                    add = addDifficulty,
                    delete = deleteDifficulty,
                });
            }
            return commands;
        }

        void DisableDeletedQuestion()
        {
            foreach (var difficulty in DataProvider.Ins.DB.DIFFICULTY)
            {
                if (ListDifficulty.Where(param => param.IDDifficulty == difficulty.IDDifficulty).ToList().Count == 0)
                    difficulty.Disabled = true;
            }
        }

        #endregion

        #region Button Hủy

        private void CancelEditingDifficulty()
        {
            ListDifficulty = null;
        }

        #endregion

        #region Datagrid Danh sách độ khó
        private ObservableCollection<DifficultyModel> _listDifficulty;
        public ObservableCollection<DifficultyModel> ListDifficulty
        {
            get
            {

                if (_listDifficulty == null)
                {
                    _listDifficulty = new ObservableCollection<DifficultyModel>((from d in DataProvider.Ins.DB.DIFFICULTY
                                                                                 where d.Disabled == false
                                                                                 select new DifficultyModel
                                                                                 {
                                                                                     IDDifficulty = d.IDDifficulty,
                                                                                     Name = d.Name
                                                                                 }));
                }
                return _listDifficulty;
            }
            set { _listDifficulty = value; OnPropertyChange("ListDifficulty"); NumberOfDifficulty = null; }
        }
        #endregion

        #endregion

        #region Class Setting

        #region Combobox Year
        BindingList<int> _year;
        public BindingList<int> Year
        {
            get
            {
                if (_year == null)
                {
                    _year = new BindingList<int>();
                    for(int i = -5; i <= 5; i++)
                    {
                        _year.Add(DateTime.Now.Year + i);
                    }
                }
                return _year;
            }
            set { _year = value; OnPropertyChange("Year"); }
        }

        int? _selectedYear;
        public int? SelectedYear
        {
            get
            {
                if (_selectedYear == null)
                    _selectedYear = DateTime.Now.Year;
                return _selectedYear;
            }
            set { _selectedYear = value; OnPropertyChange("SelectedYear"); ListClass = null; }
        }
        #endregion

        #region Combobox Selected Semester
        BindingList<int> _semester;
        public BindingList<int> Semester
        {
            get
            {
                if (_semester == null)
                {
                    _semester = new BindingList<int>();
                    _semester.Add(1);
                    _semester.Add(2);
                }
                return _semester;
            }
            set { _semester = value; OnPropertyChange("Semester"); }
        }

        int? _selectedSemester;
        public int? SelectedSemester
        {
            get
            {
                if (_selectedSemester == null)
                    _selectedSemester = Semester[0];
                return _selectedSemester;
            }
            set { _selectedSemester = value; OnPropertyChange("Selectedsemester"); ListClass = null; }
        }
        #endregion

        #region Datagrid danh sách lớp học
        // List holding a source of class without filter with Year or Semester
        private ObservableCollection<ClassModel> _listClassSource;
        public ObservableCollection<ClassModel> ListClassSource
        {
            get
            {
                if (_listClassSource == null)
                    _listClassSource = new ObservableCollection<ClassModel>(from c in DataProvider.Ins.DB.CLASS
                                                                            join s in DataProvider.Ins.DB.SUBJECT on c.IDSubject equals s.IDSubject
                                                                            select new ClassModel
                                                                            {
                                                                                IDClass = c.IDClass,
                                                                                ClassName = c.Name,
                                                                                Subject = s,
                                                                                Year = c.Year,
                                                                                Semester = c.Semester,
                                                                                Username = c.Username
                                                                            });
                return _listClassSource;
            }
            set { _listClassSource = value; OnPropertyChange("ListClassSource"); _listClass = null; ListClass = null; }
        }

        private ObservableCollection<ClassModel> _listClass;
        public ObservableCollection<ClassModel> ListClass
        {
            get
            {
                if (_listClass == null)
                {
                    _listClass = new ObservableCollection<ClassModel>(ListClassSource.Where(param => param.Year == SelectedYear && param.Semester == SelectedSemester).ToList());
                    _listClass.CollectionChanged += View_CollectionChanged;
                    // Pick up classes out of source, will return it later
                    foreach (var Class in _listClass)
                    {
                        ListClassSource.Remove(Class);
                    }
                }
                return _listClass;
            }
            set 
            {
                // return classes to source
                if(_listClass != null)
                {
                    foreach (var Class in _listClass)
                    {
                        ListClassSource.Add(Class);
                    }
                }
                _listClass = value; 
                OnPropertyChange("ListClass"); 
                NumberOfClass = null; 
            }
        }

        #endregion

        #region Save Changed Class
        void SaveChangeClass()
        {
            List<DatabaseCommand> commands = GenerateCommandsClass();
            ViewModelMediator.Ins.Receive(this, commands);
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
            {
                return sb.ToString().ToLower();
            }
            return sb.ToString();
        }

        List<DatabaseCommand> GenerateCommandsClass()
        {
            List<DatabaseCommand> commands = new List<DatabaseCommand>();

            List<ClassModel> FullListClasses = ListClassSource.ToList();
            FullListClasses.AddRange(ListClass.ToList());

            foreach (var Class in FullListClasses)
            {
                var ClassInDB = DataProvider.Ins.DB.CLASS.Where(param => param.IDClass == Class.IDClass).ToList();
                CLASS deleteDifficulty = ClassInDB.Count() == 0 ? null : ClassInDB[0];
                CLASS addDifficulty = new CLASS
                {
                    IDClass = Class.IDClass == null ? RandomString(10, false) : Class.IDClass,
                    IDSubject = Class.Subject.IDSubject,
                    Semester = Class.Semester,
                    Year = Class.Year,
                    Name = Class.ClassName,
                    Username = "01",
                };
                commands.Add(new DatabaseCommand
                {
                    add = addDifficulty,
                    delete = deleteDifficulty,
                });
            }

            foreach (var Class in DataProvider.Ins.DB.CLASS)
            {
                if (FullListClasses.Where(param => param.IDClass == Class.IDClass).ToList().Count == 0)
                    commands.Add(new DatabaseCommand
                    {
                        add = null,
                        delete = Class
                    });
            }

            return commands;
        }
        #endregion

        #region Cancel Edingting

        void CancelEditingClass()
        {
            SelectedYear = null;
            SelectedSemester = null;
            ListClassSource = null;
        }

        #endregion

        BindingList<SUBJECT> _listSubjectForClass;
        public BindingList<SUBJECT> ListSubjectForClass
        {
            get
            {
                if (_listSubjectForClass == null)
                    _listSubjectForClass = new BindingList<SUBJECT>(DataProvider.Ins.DB.SUBJECT.ToList());
                return _listSubjectForClass;
            }
            set { ListSubjectForClass = value; OnPropertyChange("ListSubjectForClass"); }
        }

        #region Textbox Số lượng lớp
        private int? _numberOfClass;
        public int? NumberOfClass
        {
            get
            {
                if (_numberOfClass == null)
                    _numberOfClass = ListClass.Count;
                return _numberOfClass;
            }
            set { _numberOfClass = value; OnPropertyChange("NumberOfClass"); }
        }
        #endregion

        #endregion

        // HANHBADCODE
        #region Subject setting

        #region DataGrid List Subject
        private ObservableCollection<SUBJECT> selectSubject()
        {
            ObservableCollection<SUBJECT> result = new ObservableCollection<SUBJECT>();

            // clone every subject so they are immutable
            foreach(var subject in DataProvider.Ins.DB.SUBJECT)
            { 
                SUBJECT clonedSubject = new SUBJECT
                {
                    IDSubject = subject.IDSubject,
                    Name = subject.Name,
                };

                result.Add(clonedSubject);
            }

            return result;
        }

        private ObservableCollection<SUBJECT> _listSubjects = null;
        public ObservableCollection<SUBJECT> ListSubject
        {
            get
            {
                // lazy initialization
                if (_listSubjects == null)
                    _listSubjects = selectSubject();

                return _listSubjects;
            }

            set
            {
                _listSubjects = value;
                OnPropertyChange("ListSubject");
            }
        }

        private void UpdateSubjectFromDB()
        {
            ListSubject = selectSubject();
        }
            #endregion

            #region button Cancel
        // when user click cancel button, just reset the DataGrid back to database models
        private void cancelSujectChangeFunction()
        {
            UpdateSubjectFromDB();
        }

            #endregion

            #region button OK
        public enum SubjectValidationMessage
        {
            Valid,
            DuplicatedID,
            ExceededMaxSubject,
            EmptyID,
            EmptyName,
        }

        // Author: CuteTN
        // Forgive me for what I'm about to do...
        // but I'm gonna check validation here!
        private ObservableCollection<SUBJECT> BackupListSubject = null;
        private void saveSubjectsFunction()
        {
            // MORECODE: pop up notification

            BackupListSubject = selectSubject();
            var msg = validateSubject();

            if(msg != SubjectValidationMessage.Valid)
                HandleInvalidInput(msg);
            else
            {
                List<DatabaseCommand> commands = createSaveSubjectsCommands(ListSubject);
                NotifyToMediator(commands);
            }
        }

        private List<DatabaseCommand> createSaveSubjectsCommands(ObservableCollection<SUBJECT> newSubjects)
        {
            List<DatabaseCommand> result = new List<DatabaseCommand>();
            ObservableCollection<SUBJECT> oldSubjects = new ObservableCollection<SUBJECT>(DataProvider.Ins.DB.SUBJECT);

            foreach(var subject in oldSubjects)
            {
                DatabaseCommand cmd = new DatabaseCommand  
                {
                    add = null,
                    delete = subject,
                };

                result.Add(cmd);
            }

            foreach(var subject in newSubjects)
            {
                if(subject.IDSubject == "" && subject.Name == "")
                    continue;

                DatabaseCommand alteredCmd = result.Find((DatabaseCommand s) => { return (s.delete as SUBJECT)?.IDSubject == subject.IDSubject; });
                
                if (alteredCmd != null)
                    alteredCmd.add = subject;
                else
                {
                    DatabaseCommand cmd = new DatabaseCommand
                    {
                        add = subject,
                        delete = null,
                    };

                    result.Add(cmd);
                }
            }

            return result;
        }

        private SubjectValidationMessage validateSubject()
        {
            // HARDCODE
            if(ListSubject.Count > 4 /* DataProvider.Ins.DB.PRINCIPLE.First().MaxNumberOfSubjects */)
                return SubjectValidationMessage.ExceededMaxSubject;

            // BRUTEFORCE
            foreach(var s1 in ListSubject)
                foreach(var s2 in ListSubject)
                    if(s1 != s2 && s1.IDSubject == s2.IDSubject)
                        return SubjectValidationMessage.DuplicatedID;

            foreach(var s in ListSubject)
            {
                bool emptyID = s.IDSubject == null || s.IDSubject == "";
                bool emptyName = s.Name == null || s.Name == "";
                if (emptyID && emptyName)
                    continue;
                if (emptyID)
                    return SubjectValidationMessage.EmptyID;
                if (emptyName)
                    return SubjectValidationMessage.EmptyName;
            }

            return SubjectValidationMessage.Valid;
        }

        #endregion

        #endregion

        // LONGCODE
        #region Principle setting
        private int _maxQuestionAmount;
        public int MaxQuestionAmount
        {
            get { return _maxQuestionAmount; }
            set { _maxQuestionAmount = value; OnPropertyChange("MaxQuestionAmount"); }
        }
        private int _maxTestDuration;
        public int MaxTestDuration
        {
            get { return _maxTestDuration; }
            set { _maxTestDuration = value; OnPropertyChange("MaxQuestionAmount"); }
        }
        private int _minTestDuration;
        public int MinTestDuration
        {
            get { return _minTestDuration; }
            set { _minTestDuration = value; OnPropertyChange("MaxQuestionAmount"); }
        }
        private int _minTestScore;
        public int MinTestScore
        {
            get { return _minTestScore; }
            set { _minTestScore = value; OnPropertyChange("MaxQuestionAmount"); }
        }
        private int _maxTestScore;
        public int MaxTestScore
        {
            get { return _maxTestScore; }
            set { _maxTestScore = value; OnPropertyChange("MaxQuestionAmount"); }
        }
        PRINCIPLE _principle;
        PRINCIPLE Principle
        {
            get {
                if (_principle == null)
                    _principle = DataProvider.Ins.DB.PRINCIPLE.FirstOrDefault();
                return _principle;
            }
            set { _principle = value; }
        }
        void LoadPrinciple()
        {
            MaxQuestionAmount = Principle?.MaxNumberOfQuestion ?? 5;
            MaxTestDuration = Principle?.MaxTimeForTest ?? 180;
            MinTestDuration = Principle?.MinTimeForTest ?? 30;
            MaxTestScore = Principle?.MaxScore ?? 10;
            MinTestScore = Principle?.MinScore ?? 0;
        }
        void CancelEditingPrinciple()
        {
            LoadPrinciple();
        }
        void SaveChangePrinciple()
        {
            List<DatabaseCommand> cmdList = new List<DatabaseCommand>();
            PRINCIPLE newPrinciple = new PRINCIPLE
            {
                MaxNumberOfQuestion = MaxQuestionAmount,
                MaxTimeForTest = MaxTestDuration,
                MinTimeForTest = MinTestDuration,
                MaxScore = MaxTestScore,
                MinScore = MinTestScore,
                id = Principle?.id ?? 0
            };
            DatabaseCommand cmd = new DatabaseCommand();
            cmd.add = newPrinciple;
            cmd.delete = Principle;
            cmdList.Add(cmd);
            ViewModelMediator.Ins.Receive(this, cmdList);
            Principle = null;
            LoadPrinciple();
        }
        #endregion

        #region Common

        #region button Cancel
        private ICommand _cancelCommand = null;
        public ICommand CancelCommand
        {
            get
            {
                if(_cancelCommand == null)
                    _cancelCommand = new RelayCommand(param => cancelFunction());
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
                OnPropertyChange("CancelCommand");
            }
        }
        
        private void cancelFunction()
        {
            cancelSujectChangeFunction();
            CancelEditingDifficulty();
            CancelEditingClass();
            CancelEditingPrinciple();
        }

        #endregion

            #region button OK
        private ICommand _okCommand = null;
        public ICommand OKCommand
        {
            get
            {
                if(_okCommand == null)
                    _okCommand = new RelayCommand(param => OKFunction() );
                return _okCommand;
            }
            set
            {
                _okCommand = value;
                OnPropertyChange("OKCommand");
            }
        }
            
        private void OKFunction()
        {
            saveSubjectsFunction();
            SaveChangeDifficulty();
            SaveChangeClass();
            SaveChangePrinciple();
            //bool nothing = ViewExtension.MessageOK()
            // NOT WORK
            // DataProvider.Ins.DB.SaveChanges();
        }

            #endregion

        #region Internal business logic (CuteTN)
        private void HandleInvalidInput(SubjectValidationMessage message)
        {
            // MORECODE
            MessageBox.Show(message.ToString());
        }

        private void NotifyToMediator(List<DatabaseCommand> commands)
        {
            ViewModelMediator.Ins.Receive(this, commands);
        }
            #endregion
        
        #endregion

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            if (commands.Any((DatabaseCommand item) => item.add is PRINCIPLE || item.delete is PRINCIPLE))
                LoadPrinciple();
            // MORECODE
        }

        public SettingViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
            ListClass.CollectionChanged += View_CollectionChanged;
            LoadPrinciple();
        }

        private void View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;
            if (e.NewItems.Count > 0)
            {
                ClassModel myObject = e.NewItems[e.NewItems.Count - 1] as ClassModel;
                if (myObject != null)
                {
                    myObject.Year = (int)SelectedYear;
                    myObject.Semester = (int)SelectedSemester;
                }
            }
        }
    }
}