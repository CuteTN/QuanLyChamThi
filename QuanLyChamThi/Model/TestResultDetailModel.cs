using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyChamThi.Model
{
    class TestResultDetailModel
    {
        string _idTestResult;
        public string IDTestResult
        {
            get { return _idTestResult; }
            set { _idTestResult = value; }
        }
        string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set { _subjectName = value; }
        }

        string _idClass;
        public string IDClass
        {
            get { return _idClass; }
            set { _idClass = value; }
        }

        string _userFullName;
        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; }
        }

        string _idTest;
        public string IDTest
        {
            get { return _idTest; }
            set { _idTest = value; }
        }

    }

    class ListTestResultDetailModel: ViewModelBase ,UserModelBase
    {
        #region Data
        ObservableCollection<TestResultDetailModel> _data;
        public ObservableCollection<TestResultDetailModel> Data
        {
            get
            {
                if (_data == null)
                    _data = new ObservableCollection<TestResultDetailModel>((from u in DataProvider.Ins.DB.TESTRESULTDETAIL
                                                                             join v in DataProvider.Ins.DB.USER on u.Username equals v.Username
                                                                             join t in DataProvider.Ins.DB.TEST on u.IDTest equals t.IDTest
                                                                             join y in DataProvider.Ins.DB.SUBJECT on t.IDSubject equals y.IDSubject
                                                                             join x in DataProvider.Ins.DB.CLASS on u.IDClass equals x.IDClass into gj
                                                                             from g in gj.DefaultIfEmpty()
                                                                             select new TestResultDetailModel
                                                                             {
                                                                                 IDTestResult = u.IDTestResult,
                                                                                 SubjectName = y.Name,
                                                                                 IDClass = g == null ? string.Empty : (g.IDClass ?? string.Empty),
                                                                                 UserFullName = v.FullName,
                                                                                 IDTest = u.IDTest
                                                                             }).ToList());

                return _data;
                
            }
            set
            {
                _data = value;
                OnPropertyChange("Data");
            }
        }
        #endregion

        #region SingleTon Implement
        private static ListTestResultDetailModel _ins;
        public static ListTestResultDetailModel Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ListTestResultDetailModel();
                    // subcribe to ViewModelMediator to receive DatabaseCommands
                    ViewModelMediator.Ins.AddUserModel(_ins);
                }
                return _ins;
            }
            set { _ins = value; }
        }

        #endregion

        /// <summary>
        /// This function used to received command for change that made to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            Data = null;

            // After update base on database, it notify all viewmodel subcribed to it
            if(collectionChanged != null)
            {
                collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// This is delagate used for notify to all other viewmodel subcribed to it when it changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void Notify(object sender, NotifyCollectionChangedEventArgs e);
        private event Notify collectionChanged;

        public void AddCollectionChangedNotified(Notify n)
        {
            // We need to seperate it because the observable collection 
            // won't call CollectionChanged when we set it
            // So we have to call changed manually
            collectionChanged += n;
            Data.CollectionChanged += new NotifyCollectionChangedEventHandler(n);
        }
        public ListTestResultDetailModel()
        {
            
        }
    }
}
