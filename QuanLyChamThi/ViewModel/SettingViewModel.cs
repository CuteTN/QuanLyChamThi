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

        ICommand _saveChangeDifficultyCommand;
        public ICommand SaveChangeDifficultyCommand
        {
            get
            {
                if(_saveChangeDifficultyCommand == null)
                {
                    _saveChangeDifficultyCommand = new RelayCommand(param => SaveChangeDifficulty());
                }
                return _saveChangeDifficultyCommand;
            }
            set { _saveChangeDifficultyCommand = value; OnPropertyChange("SaveChangeDifficultyCommand"); }
        }

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
        private ObservableCollection<ClassModel> _listClass;
        public ObservableCollection<ClassModel> ListClass
        {
            get
            {
                if (_listClass == null)
                    _listClass = new ObservableCollection<ClassModel>(from c in DataProvider.Ins.DB.CLASS
                                                                      join s in DataProvider.Ins.DB.SUBJECT on c.IDSubject equals s.IDSubject
                                                                      select new ClassModel
                                                                      {
                                                                          IDClass = c.IDClass,
                                                                          Subject = s
                                                                      });
                return _listClass;
            }
            set { _listClass = value; OnPropertyChange("ListClass"); NumberOfClass = null; }
        }

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

    }
}
