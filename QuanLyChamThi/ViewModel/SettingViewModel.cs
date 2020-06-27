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

namespace QuanLyChamThi.ViewModel
{
    class SettingViewModel : ViewModelBase, UserModelBase
    {
        // BADCODE
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
