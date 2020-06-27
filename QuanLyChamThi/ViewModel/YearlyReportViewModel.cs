using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class YearlyReportViewModel : ViewModelBase, UserModelBase
    {
        #region Combobox year
        private void checkValidYearRange()
        {
            if(YearMin > YearMax)
                throw new Exception("YearMin have to be less or equal to YearMax");
        }

        private void onYearRangeChange()
        {
            _listYear = createListYear();
        }

        private int _yearMin = 1975;
        public int YearMin
        {
            get { return _yearMin; }
            set
            {
                if(_yearMin != value)
                {
                    if(value <= YearMax)
                        _yearMin = value;
                    onYearRangeChange();
                }
            }
        }

        private int _yearMax = 2020;
        public int YearMax
        {
            get { return _yearMax; }
            set
            {
                if(_yearMax != value)
                {
                    if(YearMin <= value)
                        _yearMax = value;
                    onYearRangeChange();
                }
            }
        }

        private List<string> createListYear()
        {
            List<string> result = new List<string>();

            // decrement i to make the the most recent year to the top
            for(int i=YearMax; i>=YearMin; i--)
                result.Add(i.ToString());    

            return result;
        }

        private List<string> _listYear = null;
        public List<string> ListYear
        {
            get
            {
                if(_listYear == null)
                {
                    _listYear = createListYear();
                }

                return _listYear;
            }
            set
            {
                _listYear = value;
                OnPropertyChange("ListYear");
            }
        }
        #endregion

        #region Selected item
        private string _strSelectedYear = null;
        public string StrSelectedYear
        {
            get
            {
                if(_strSelectedYear == null)
                    _strSelectedYear = YearMax.ToString();
                return _strSelectedYear;
            }
            set
            {
                _strSelectedYear = value;
                OnPropertyChange("StrSelectedYear");
            }
        }

        public int SelectedYear
        {
            get
            {
                if(StrSelectedYear == null)
                    return 0;
                try
                { 
                    return int.Parse(StrSelectedYear);
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region DataGrid Report
        private ObservableCollection<SubjectYearlyReportModel> _listReport;
        public ObservableCollection<SubjectYearlyReportModel> ListReport
        {
            get
            {
                if (_listReport == null)
                {
                    _listReport = model.Data;
                }
                return _listReport;
            }
            set 
            { 
                _listReport = value; 
                OnPropertyChange("ListReport");
            }
        }
        #endregion

        #region button Make Report
        private ICommand _makeReportCommand = null;
        public ICommand MakeReportCommand
        {
            get
            {
                if(_makeReportCommand == null)
                    _makeReportCommand = new RelayCommand(param => MakeReportFunction(), null);
                return _makeReportCommand;
            }
            set
            {
                _makeReportCommand = value;
                OnPropertyChange("MakeReportCommand");
            }
        }

        private void MakeReportFunction()
        {
            model.Year = this.SelectedYear;
            model.UpdateFromDB();
            ListReport = model.Data;
        }
        #endregion

        #region Internal business logic
        YearlyReportModel model = null;
        #endregion

        #region tbTotalTest and tbTotalTestResult
        public int TotalTestCount
        {
            get { return model.TotalTestCount; }
            set { /* can't set this */ }
        }
        public int TotalTestResultCount
        {
            get { return model.TotalTestResultCount; }
            set { /* can't set this */ }
        }

        public void OnTotalTestCountChange(Object sender, EventArgs args)
        {
            // just to be sure...
            if(sender != model)
                return;
            OnPropertyChange("TotalTestCount");
        }

        public void OnTotalTestResultCountChange(Object sender, EventArgs args)
        {
            // just to be sure...
            if(sender != model)
                return;
            OnPropertyChange("TotalTestResultCount");
        }
        #endregion

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // MORECODE
        }

        private void InitializeModel()
        {
            model = new YearlyReportModel(SelectedYear);

            model.TotalTestCountChange += OnTotalTestCountChange;
            model.TotalTestResultCountChange += OnTotalTestResultCountChange;
        }

        public YearlyReportViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
            InitializeModel();
        }
    }
}
