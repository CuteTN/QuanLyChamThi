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
using QuanLyChamThi.View;

namespace QuanLyChamThi.ViewModel
{
    class SettingViewModel : ViewModelBase, UserModelBase
    {
        // THANHCODE

        #region Difficulty

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

        ICommand _saveChangeDifficultyCommand;
        public ICommand SaveChangeDifficultyCommand
        {
            get
            {
                if (_saveChangeDifficultyCommand == null)
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
            //bool nothing = ViewExtension.MessageOK()
            // NOT WORK
            DataProvider.Ins.DB.SaveChanges();
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
            // MORECODE
        }

        public SettingViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
    }
}