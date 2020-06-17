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

namespace QuanLyChamThi.ViewModel
{
    class TestResultDetailViewModel: ViewModelBase
    {

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
            set { _listSubject = value; }
        }

        private SUBJECT _selectedSubject;
        public SUBJECT SelectedSubject
        {
            get
            {
                if(_selectedClass != null)
                {
                    _selectedSubject = (from u in DataProvider.Ins.DB.SUBJECT
                                        where u.IDSubject == _selectedClass.IDSubject
                                        select u).ToList().First();
                }
                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                OnPropertyChange("ListClass");
                OnPropertyChange("ListTestID");
            }
        }
        #endregion

        #region Combobox Class

        private BindingList<CLASS> _listClass;
        public BindingList<CLASS> ListClass
        {
            get
            {
                if (_selectedSubject != null)
                {
                    _listClass = new BindingList<CLASS>((from u in DataProvider.Ins.DB.CLASS
                                                         where u.IDSubject == _selectedSubject.IDSubject
                                                         select u).ToList());
                }
                else
                {
                    _listClass = new BindingList<CLASS>(DataProvider.Ins.DB.CLASS.ToList());
                }

                return _listClass;
            }
        }

        private CLASS _selectedClass;
        public CLASS SelectedClass
        {
            get
            {
                return _selectedClass;
            }
            set
            {
                _selectedClass = value;
                OnPropertyChange("SelectedSubject");
                OnPropertyChange("ListTestID");
            }
        }

        #endregion

        #region Combobox IDTest

        private BindingList<TEST> _listTestID;
        public BindingList<TEST> ListTestID
        {
            get
            {
                if (_selectedSubject != null)
                {
                    _listTestID = new BindingList<TEST>((from u in DataProvider.Ins.DB.TEST
                                                         where u.IDSubject == _selectedSubject.IDSubject
                                                         select u).ToList());
                }
                else
                {
                    _listTestID = new BindingList<TEST>(DataProvider.Ins.DB.TEST.ToList());
                }
                return _listTestID;
            }
            set
            {
                _listTestID = value;
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
            }
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
            string IDTestResult = SelectedClass.IDClass + "_" + SelectedTestID.IDTest;

            DataProvider.Ins.DB.TESTRESULTDETAIL.Add(new TESTRESULTDETAIL
            {
                IDTestResult = IDTestResult,
                IDClass      = _selectedClass.IDClass,
                IDTest       = _selectedTestID.IDTest,
                Username     = "01",
                Note         = ""
            });

            foreach(var testResult in _listTestResult)
            {
                DataProvider.Ins.DB.TESTRESULT.Add(new TESTRESULT
                {
                    IDTestResult = IDTestResult,
                    IDStudent = testResult.StudentID,
                    ScoreNumber = testResult.ScoreNumber,
                    ScoreString = testResult.ScoreString,
                    Note = testResult.Note
                });
            }

            try
            {
                DataProvider.Ins.DB.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var x = e.ToString();
            }

            string UserFullName = DataProvider.Ins.DB.USER.Where((USER user) => user.Username == "01").ToList()[0].FullName;

            ListTestResultDetailModel.Ins.Data.Add(new TestResultDetailModel
            {
                IDTestResult = IDTestResult,
                IDClass = _selectedClass.IDClass,
                IDTest = _selectedTestID.IDTest,
                UserFullName = UserFullName,
                SubjectName = SelectedSubject.Name
            });
        }

        bool CanSave()
        {
            return true;
        }

        #endregion
        public TestResultDetailViewModel()
        {
            
        }
    }
}
