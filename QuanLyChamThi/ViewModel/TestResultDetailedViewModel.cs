using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Windows;
using QuanLyChamThi.View;

namespace QuanLyChamThi.ViewModel
{
    class TestResultDetailedViewModel: ViewModelBase
    {

        #region Datagrid ListTestResultDetail
        /// <summary>
        /// This is a list that binding with the view, it has a filter of its own
        /// Its source is ListTestResultDetailModel singleton, every change in ListTestResultDetailModel 
        /// will call this list to update
        /// </summary>
        ObservableCollection<TestResultDetailModel> _listTestResultDetail;
        public ObservableCollection<TestResultDetailModel> ListTestResultDetail
        {
            get
            {
                _listTestResultDetail = new ObservableCollection<TestResultDetailModel>(ListTestResultDetailModel.Ins.Data.Where(param => Filter(param)));
                return _listTestResultDetail;
            }
            set
            {
                _listTestResultDetail = value;
            }
        }

        IList<TestResultDetailModel> _selectedResultDetails;
        public IList<TestResultDetailModel> SelectedResultDetails
        {
            get
            {
                if (_selectedResultDetails == null)
                    _selectedResultDetails = new List<TestResultDetailModel>();
                return _selectedResultDetails;
            }
            set
            {
                _selectedResultDetails = value;
                OnPropertyChange("SelectedResultDetail");
                OnPropertyChange("NumberOfSelectedResultDetail");
            }
        }

        private int _numberOfSelectedResultDetail;
        public int NumberOfSelectedResultDetail
        {
            get { _numberOfSelectedResultDetail = SelectedResultDetails.Count; return _numberOfSelectedResultDetail; }
            set { _numberOfSelectedResultDetail = value; OnPropertyChange("NumberOfSelectedResultDetail"); }
        }

        private ICommand _deselecteResultDetailCommand;
        public ICommand DeselecteResultDetailCommand
        {
            get
            {
                if (_deselecteResultDetailCommand == null)
                    _deselecteResultDetailCommand = new RelayCommand(param => DeselecteResultDetail());
                return _deselecteResultDetailCommand;
            }
            set { _deselecteResultDetailCommand = value; }
        }

        private void DeselecteResultDetail()
        {
            SelectedResultDetails = null;
        }
        #endregion

        #region Button Xóa bài chấm được chọn
        ICommand _deleteSelectedResultDetailCommand;
        public ICommand DeleteSelectedResultDetailCommand
        {
            get
            {
                if (_deleteSelectedResultDetailCommand == null)
                    _deleteSelectedResultDetailCommand = new RelayCommand(p => DeleteSelectedResultDetail(), p => CanDeleteSelectedResultDetail());
                return _deleteSelectedResultDetailCommand;
            }
            set
            {
                _deleteSelectedResultDetailCommand = value;
            }
        }
        void DeleteSelectedResultDetail()
        {
            if (ViewExtension.Confirm(null, "Bạn có chắc muốn xóa những bài chấm được chọn không?") == 0)
                return;

            if (SelectedResultDetails == null)
                return;
            List<DatabaseCommand> commands = new List<DatabaseCommand>();
            foreach (var _selectedResultDetail in SelectedResultDetails)
            {
                var deletedResultDetail = DataProvider.Ins.DB.TESTRESULTDETAIL.Find(_selectedResultDetail.IDTestResult);
                commands.Add(new DatabaseCommand
                {
                    add = null,
                    delete = deletedResultDetail
                });
            }
            ViewModelMediator.Ins.Receive(this, commands);
        }

        bool CanDeleteSelectedResultDetail()
        {
            return SelectedResultDetails.Count > 0;
        }
        #endregion

        #region Button Thêm một bài chấm
        ICommand _addNewTestResultDetailCommand;
        public ICommand AddNewTestResultDetailCommand
        {
            get
            {
                if (_addNewTestResultDetailCommand == null)
                    _addNewTestResultDetailCommand = new RelayCommand(param => AddNewTestResultDetail());
                return _addNewTestResultDetailCommand;
            }
            set { _addNewTestResultDetailCommand = value; OnPropertyChange("AddNewTestResultDetailCommad"); }
        }

        void AddNewTestResultDetail()
        {
            MainWindowViewModel.Ins.StartEditingTestResult(null);
        }
        #endregion

        #region Button Sửa bài chấm được chọn
        ICommand _editSelectedTestResultDetailCommand;
        public ICommand EditSelectedTestResultDetailCommand
        {
            get
            {
                if (_editSelectedTestResultDetailCommand == null)
                    _editSelectedTestResultDetailCommand = new RelayCommand(param => EditSelectedTestResultDetail(), param => CanEditSelectedTestResultDetail());
                return _editSelectedTestResultDetailCommand;
            }
            set
            {
                _editSelectedTestResultDetailCommand = value;
            }
        }
        void EditSelectedTestResultDetail()
        {
            if (SelectedResultDetails.Count != 1)
                return;
            else
            {
                var selectedResultDetail = SelectedResultDetails[0];
                MainWindowViewModel.Ins.StartEditingTestResult((from u in DataProvider.Ins.DB.TESTRESULTDETAIL
                                                                where u.IDTestResult == selectedResultDetail.IDTestResult
                                                                select u).Single());
            }
        }

        bool CanEditSelectedTestResultDetail()
        {
            return SelectedResultDetails.Count != 1 ? false : true;
        }
        #endregion

        private bool Filter(object item)
        {
            TestResultDetailModel test = item as TestResultDetailModel;

            ///////// PUT FILER HERE ///////////////
            return true;
        }

        void refresh(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChange("ListTestResultDetail");
        }

        public TestResultDetailedViewModel()
        {
            ListTestResultDetailModel.Ins.AddCollectionChangedNotified(refresh);
        }
    }
}
