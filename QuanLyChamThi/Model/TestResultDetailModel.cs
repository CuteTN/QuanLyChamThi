using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    class ListTestResultDetailModel
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
                                                                             join x in DataProvider.Ins.DB.CLASS on u.IDClass equals x.IDClass
                                                                             join y in DataProvider.Ins.DB.SUBJECT on x.IDSubject equals y.IDSubject
                                                                             select new TestResultDetailModel
                                                                             {
                                                                                 IDTestResult = u.IDTestResult,
                                                                                 SubjectName = y.Name,
                                                                                 IDClass = x.IDClass,
                                                                                 UserFullName = v.FullName,
                                                                                 IDTest = u.IDTest
                                                                             }).ToList());

                return _data;
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
                }
                return _ins;
            }
            set { _ins = value; }
        }

        private ListTestResultDetailModel() { }
        #endregion
    }
}
