using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace QuanLyChamThi.Model
{
    class YearlyReportModel
    {
        public YearlyReportModel(int year = -1)
        {
            this.Year = year;
        }

        private int _year = -1;
        public int Year
        {
            get { return _year; }
            set 
            { 
                if(_year != value)
                    _data = null;
                _year = value;
            }
        }
        
        #region Data
        private ObservableCollection<SubjectYearlyReportModel> selectData(int year)
        {
            // Author: Tuong
            var subjects = DataProvider.Ins.DB.SUBJECT.ToList().Select((SUBJECT src) =>
            {
                var tests = DataProvider.Ins.DB.TEST.Where((test) => (test.IDSubject == src.IDSubject) && (test.Year == year) );
                SubjectYearlyReportModel trg = new SubjectYearlyReportModel
                {
                    IDSubject = src.IDSubject,
                    TestCount = tests.Count(),
                    TestResultCount = (from u in tests 
                                       join v in DataProvider.Ins.DB.TESTRESULTDETAIL 
                                       on  u.IDTest equals v.IDTest
                                       select v.IDTestResult).Count()
                };
                return trg;
            });

            ObservableCollection<SubjectYearlyReportModel> result = new ObservableCollection<SubjectYearlyReportModel>(subjects.ToList());
            return result;
        }

        private ObservableCollection<SubjectYearlyReportModel> _data;
        public ObservableCollection<SubjectYearlyReportModel> Data
        {
            get
            {
                // lazy initialization
                if (_data == null)
                { 
                    _data = selectData(Year);
                    fillTotal();
                }

                return _data;
            }

            set
            {
                _data = value;
                if(value != null)
                    fillTotal();
            }
        }

        private void fillTotal()
        {
            // BADCODE
            int totalTestCount = 0;
            int totalTestResultCount = 0;

            foreach(var report in _data)
            { 
                totalTestCount += report.TestCount;
                totalTestResultCount += report.TestResultCount;
            }

            foreach(var report in _data)
            {
                report.TotalTestCount = totalTestCount;
                report.TotalTestResultCount = totalTestResultCount;
            }
        }
        #endregion


    }
}
