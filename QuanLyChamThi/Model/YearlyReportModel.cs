using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                _year = value;
                Data = selectData(_year);
            }
        }
        
        #region Data
        private ObservableCollection<SubjectYearlyReportModel> selectData(int year)
        {
            // user is not yet implemented
            var query = from t in DataProvider.Ins.DB.TEST
                        where t.Year == year
                        join d in DataProvider.Ins.DB.TESTRESULTDETAIL on t.IDTest equals d.IDTest
                        join r in DataProvider.Ins.DB.TESTRESULT on d.IDTestResult equals r.IDTestResult
                        select new { t.IDSubject, t.IDTest, r.IDTestResult } into j
                        group j by new { j.IDSubject } into g
                        select new SubjectYearlyReportModel
                        {
                            IDSubject = g.Key.IDSubject,
                            TestCount = g.Select(j => j.IDTest).Distinct().Count(),
                            TestResultCount = g.Select(j => j.IDTestResult).Distinct().Count(),
                        };

            ObservableCollection<SubjectYearlyReportModel> result = new ObservableCollection<SubjectYearlyReportModel>(query.ToList());
            
            return result;
        }

        private ObservableCollection<SubjectYearlyReportModel> _data;
        public ObservableCollection<SubjectYearlyReportModel> Data
        {
            get
            {
                // lazy initialization
                if (_data == null)
                    _data = selectData(Year);

                return _data;
            }

            set
            {
                _data = value;
            }
        }

        private void fillTotal()
        {
            int totalTestCount = 0;
            int totalTestResultCount = 0;

            // foreach(var )

        }
        #endregion


    }
}
