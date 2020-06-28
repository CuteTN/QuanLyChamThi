using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;

namespace QuanLyChamThi.ViewModel
{
    class SettingViewModel: ViewModelBase
    {
        #region Difficulty

        #region Textbox Số lượng độ khó
        private int? _numberOfDifficulty;
        public int? NumberOfDifficulty
        {
            get 
            {
                if(_numberOfDifficulty == null)
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
            for(int i = ListDifficulty.Count; i != NumberOfDifficulty;)
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

            foreach(var difficulty in ListDifficulty)
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
            foreach(var difficulty in DataProvider.Ins.DB.DIFFICULTY)
            {
                if (ListDifficulty.Where(param => param.IDDifficulty == difficulty.IDDifficulty).ToList().Count == 0)
                    difficulty.Disabled = true;
            }
        }

        #endregion

        #region Button Hủy
        private ICommand _cancelEditingCommand;
        public ICommand CancelEditingCommand
        {
            get
            {
                if (_cancelEditingCommand == null)
                    _cancelEditingCommand = new RelayCommand(param => CancelEditingDifficulty());
                return _cancelEditingCommand;
            }
            set
            {
                _cancelEditingCommand = value; OnPropertyChange("CancelEditingCommand");
            }
        }

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

        #region Subject

        #endregion

        #region Class

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
            set { _listClassSource = value; OnPropertyChange("ListClassSource"); ListClass = null; }
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

        ICommand _saveChangeCommand;
        public ICommand SaveChangeCommand
        {
            get
            {
                if (_saveChangeCommand == null)
                {
                    _saveChangeCommand = new RelayCommand(param => SaveChange());
                }
                return _saveChangeCommand;
            }
            set { _saveChangeCommand = value; OnPropertyChange("SaveChangeDifficultyCommand"); }
        }

        void SaveChange()
        {
            SaveChangeDifficulty();
            SaveChangeClass();
        }


        public SettingViewModel()
        {
            ListClass.CollectionChanged += View_CollectionChanged;
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
