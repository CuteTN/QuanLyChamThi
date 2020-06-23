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

namespace QuanLyChamThi.ViewModel
{
    class TestResultDetailedViewModel: ViewModelBase
    {
        /// <summary>
        /// This is a list that binding with the view, it has a filter of its own
        /// Its source is ListTestResultDetailModel singleton, every change in ListTestResultDetailModel 
        /// will call this list to update
        /// </summary>
        ObservableCollection<TestResultDetailModel> _listTestResultDetailView;
        public ObservableCollection<TestResultDetailModel> ListTestResultDetailView
        {
            get
            {
                // _listTestResultDetailView = new ObservableCollection<TestResultDetailModel>(ListTestResultDetail.Cast<TestResultDetailModel>().ToList());
                _listTestResultDetailView = new ObservableCollection<TestResultDetailModel>(ListTestResultDetailModel.Ins.Data.Where((TestResultDetailModel param) => Filter(param)));
                return _listTestResultDetailView;
            }
            set
            {
                _listTestResultDetailView = value;
            }
        }

        TestResultDetailModel _selectedResultDetail;
        public TestResultDetailModel SelectedResultDetail
        {
            get
            {
                return _selectedResultDetail;
            }
            set
            {
                _selectedResultDetail = value;
            }
        }

        ICommand _deleteSelectedResultDetailCommand;
        public ICommand DeleteSelectedResultDetailCommand
        {
            get
            {
                if (_deleteSelectedResultDetailCommand == null)
                    _deleteSelectedResultDetailCommand = new RelayCommand(p => DeleteSelectedResultDetail());
                return _deleteSelectedResultDetailCommand;
            }
            set
            {
                _deleteSelectedResultDetailCommand = value;
            }
        }
        void DeleteSelectedResultDetail()
        {
            if (_selectedResultDetail == null)
                return;
            List<DatabaseCommand> commands = new List<DatabaseCommand>();
            var x = DataProvider.Ins.DB.TESTRESULTDETAIL.Find(_selectedResultDetail.IDTestResult);
            var entityType = ObjectContext.GetObjectType(x.GetType());
            commands.Add(new DatabaseCommand
            {
                add = null,
                delete = x
            });
            ViewModelMediator.Ins.Receive(this, commands);
        }

        private bool Filter(object item)
        {
            TestResultDetailModel test = item as TestResultDetailModel;

            ///////// PUT FILER HERE ///////////////
            return test.SubjectName == "Nhập môn lập trình";
        }

        void refresh(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChange("ListTestResultDetailView");
            //MessageBox.Show("");
        }

        public TestResultDetailedViewModel()
        {
            ListTestResultDetailModel.Ins.AddCollectionChangedNotified(refresh);
        }
    }
}
