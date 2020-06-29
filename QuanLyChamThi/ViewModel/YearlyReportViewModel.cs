using Microsoft.Office.Interop.Excel;
using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using QuanLyChamThi.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
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
                isUpToDate = false;
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
            refresh();
        }
        #endregion

        #region button Save As File
        // this is for exporting to excel
        private List<List<string>> createReportTable(ObservableCollection<SubjectYearlyReportModel> listReport)
        {
            // BADCODE
            List<List<string>> result = new List<List<string>>();

            // Index - Subject - NOTest - NOTestResult - RTest - RTestsult
            
            // header
            result.Add(new List<string>());
            string[] headers = "STT/Tên môn học/Số lượng đề thi/Số lượng bài chấm/Tỉ lệ đề thi/Tỉ lệ bài chấm".Split('/');
            result[0].AddRange(headers);

            // content
            foreach(var report in listReport)
            {
                result.Add(new List<string>());

                result.Last().Add(report.Index.ToString());
                result.Last().Add(report.Subject);
                result.Last().Add(report.TestCount.ToString());
                result.Last().Add(report.TestResultCount.ToString());
                result.Last().Add(report.TestPercentage);
                result.Last().Add(report.TestResultPercentage);
            }

            return result;
        }
        
        private void SaveAsFileFunction()
        {
            remindUpdate();

            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Microsoft Office Excel documents|*.xlsx";
            bool? userAcceptSaving = dlg.ShowDialog();

            if (userAcceptSaving==null || !userAcceptSaving.Value)
                return;

            string fileName = dlg.FileName;

            var dataToSave = createReportTable(ListReport);
            bool saveSuccess = QuanLyChamThi.Utilities.ExcelExporter.Export(dataToSave, fileName);

            if(! saveSuccess)
            {
                ViewExtension.MessageOK(null, "Lỗi: lưu tệp tin không thành công", ViewExtension.MessageType.Error);
                return;
            }
            else
            {
                ViewExtension.Message(null, "Thông báo: tệp tin được lưu thành công!", "", ViewExtension.MessageType.Notification);
            }

        }

        private ICommand _saveAsFileCommand = null;
        public ICommand SaveAsFileCommand
        {
            get
            {
                if (_saveAsFileCommand == null)
                    _saveAsFileCommand = new RelayCommand(param => SaveAsFileFunction(), null);
                return _saveAsFileCommand;
            }
            set
            {
                _saveAsFileCommand = value;
                OnPropertyChange("SaveAsFileCommand");
            }
        }

        #endregion

        #region button Print
        private void printFunction(System.Windows.Controls.DataGrid dataGrid)
        {
            remindUpdate();

            System.Windows.Controls.PrintDialog dlg = new System.Windows.Controls.PrintDialog();
            dlg.PrintVisual(dataGrid, "Báo cáo năm");
            dlg.ShowDialog();
        }

        private ICommand _printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if(_printCommand == null)
                    _printCommand = new RelayCommand(param => printFunction(param as System.Windows.Controls.DataGrid));
                return _printCommand;
            }
            set
            {
                _printCommand = value;
                OnPropertyChange("PrintCommand");
            }
        }
        #endregion

        #region Internal business logic
        YearlyReportModel model = null;
        private bool isUpToDate = true;

        private void remindUpdate()
        {
            if(isUpToDate)
                return;
            int veResult = ViewExtension.Confirm(null, "Cảnh báo: dữ liệu chưa được cập nhật. Bạn có muốn cập nhật dữ liệu không?");

            if(veResult == 1)
                refresh();
        }

        private void refresh()
        {
            model.Year = this.SelectedYear;
            model.UpdateFromDB();
            ListReport = model.Data;
            isUpToDate = true;
        }
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
            isUpToDate = false;
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
