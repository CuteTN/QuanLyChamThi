using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.Collections.ObjectModel;

namespace QuanLyChamThi.ViewModel
{
    class TestResultViewModel
    {
        ObservableCollection<TestResultDetailModel> _listTestResultDetail;
        public ObservableCollection<TestResultDetailModel> ListTestResultDetail
        {
            get
            {
                if (_listTestResultDetail == null)
                    _listTestResultDetail = new ObservableCollection<TestResultDetailModel>((from u in DataProvider.Ins.DB.TESTRESULTDETAIL
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
                return _listTestResultDetail;
            }
            set { _listTestResultDetail = value; }
        }
    }
}
