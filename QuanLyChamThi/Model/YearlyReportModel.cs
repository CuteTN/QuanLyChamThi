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
                    fillIndex();
                }

                return _data;
            }

            set
            {
                _data = value;
                if(value != null)
                { 
                    fillTotal();
                    fillIndex();
                }
            }
        }
        #endregion

        #region internal business logic
        private int _totalTestCount = 0;
        public int TotalTestCount
        {
            get { return _totalTestCount; }
            private set { _totalTestCount = value; TotalTestCountChange?.Invoke(this, null); }
        }

        public int _totalTestResultCount = 0;
        public int TotalTestResultCount
        {
            get { return _totalTestResultCount; }
            private set { _totalTestResultCount = value; TotalTestResultCountChange?.Invoke(this, null); }
        }

        private void fillTotal()
        {
            // BADCODE
            TotalTestCount = 0;
            TotalTestResultCount = 0;

            // recalculate the whole thing
            foreach(var report in _data)
            { 
                TotalTestCount += report.TestCount;
                TotalTestResultCount += report.TestResultCount;
            }

            // update to each subject report
            foreach(var report in _data)
            {
                report.TotalTestCount = TotalTestCount;
                report.TotalTestResultCount = TotalTestResultCount;
            }
        }

        private void fillIndex()
        {
            int index = 1;

            foreach(var report in _data)
            {
                report.Index = index;
                index++;
            }
        }

        public void UpdateFromDB()
        {
            // just set _data = null, getter of Data will auto lazily update DB
            _data = null;
        }
        #endregion

        #region message passing
        public event EventHandler TotalTestCountChange;
        public event EventHandler TotalTestResultCountChange;
        #endregion
    }
}
