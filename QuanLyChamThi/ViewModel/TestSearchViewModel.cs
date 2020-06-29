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
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class TestSearchViewModel : ViewModelBase, UserModelBase
    {
        public TestSearchViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
            Search();
        }

        #region Filter
        private class FilterState
        {
            public string TestID;
            public string TestDuration;
            public DateTime? date;

            public bool FilterFunction(TEST item)
            {
                if (TestID != "" && item.IDTest != TestID)
                    return false;
                try
                {
                    if (item.TimeForTest != int.Parse(TestDuration))
                        return false;
                }
                catch (Exception) { }
                if (date != null && item.DateOfTest?.Date != date?.Date)
                    return false;
                return true;
            }
        }
        private FilterState _filter;
        private FilterState Filter
        {
            get
            {
                if (_filter == null)
                    _filter = new FilterState();
                return _filter;
            }
            set { _filter = value; }
        }

        private string _filterTestID;
        public string FilterTestID
        {
            get
            {
                if (_filterTestID == null)
                    _filterTestID = "";
                return _filterTestID;
            }
            set { _filterTestID = value; }
        }
        private string _filterTestDuration;
        public string FilterTestDuration
        {
            get
            {
                if (_filterTestDuration == null)
                    _filterTestDuration = "";
                return _filterTestDuration;
            }
            set { _filterTestDuration = value; }
        }
        private DateTime? _filterTestDate;
        public DateTime? FilterTestDate
        {
            get{ return _filterTestDate; }
            set { _filterTestDate = value; }
        }
        public int NumberSelectedItem
        {
            get { return SelectedTest.Count; }
            set { OnPropertyChange("NumberSelectedItem"); }
        }
        #endregion

        #region Data
        private BindingList<TEST> _test;
        public BindingList<TEST> Test
        {
            get
            {
                if (_test == null)
                    _test = new BindingList<TEST>((from u in DataProvider.Ins.DB.TEST select u).ToList());
                return _test;
            }
            set { _test = value; }
        }

        private ObservableCollection<TestSearchModel> _listTest;
        public ObservableCollection<TestSearchModel> ListTest
        {
            get
            {
                if (_listTest == null)
                    _listTest = new ObservableCollection<TestSearchModel>();
                return _listTest;
            }
            set { _listTest = value; OnPropertyChange("ListTest"); }
        }
        private IList<TestSearchModel> _selectedTest;
        public IList<TestSearchModel> SelectedTest
        {
            get
            {
                if (_selectedTest == null)
                    _selectedTest = new List<TestSearchModel>();
                return _selectedTest;
            }
            set { _selectedTest = value;
                OnPropertyChange("SelectedTest");
                OnPropertyChange("NumberSelectedItem");
            }
        }
        #endregion

        #region Search Button
        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand(param => Search());
                return _searchCommand;
            }
            set { _searchCommand = value; }
        }

        private void Search()
        {
            Filter.TestDuration = FilterTestDuration;
            Filter.TestID = FilterTestID;
            Filter.date = FilterTestDate;

            Search(Filter);
        }

        private void Search(FilterState filter)
        {
            ListTest.Clear();
            var test = Test.Where(filter.FilterFunction);
            foreach (var item in test)
            {
                TestSearchModel newItem = new TestSearchModel();
                newItem.TestDate = item.DateOfTest?.ToString()??"Không có thông tin";
                newItem.TestDuration = item.TimeForTest;
                newItem.TestID = item.IDTest;
                newItem.pSource = item;

                ListTest.Add(newItem);
            }
        }
        #endregion

        #region NewTestButton
        
        private ICommand _newTestCommand;
        public ICommand NewTestCommand
        {
            get
            {
                if (_newTestCommand == null)
                    _newTestCommand = new RelayCommand(param => MainWindowViewModel.Ins.SwitchView(9, ""));
                return _newTestCommand;
            }
            set { _newTestCommand = value; }
        }
        #endregion

        #region EditTestCommand
        private ICommand _editTestCommand;
        public ICommand EditTestCommand
        {
            get
            {
                if (_editTestCommand == null)
                    _editTestCommand = new RelayCommand(param => EditTestButton());
                return _editTestCommand;
            }
            set { _editTestCommand = value; }
        }

        void EditTestButton()
        {
            if (SelectedTest.Count != 1)
            {
                ViewExtension.MessageOK(null, "Lỗi: vui lòng chọn 1 đề thi để chỉnh sửa", ViewExtension.MessageType.Error);
                return;
            }

            MainWindowViewModel.Ins.SwitchView(9, SelectedTest[0].TestID);
        }
        #endregion

        #region Delete Button
        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => Delete());
                return _deleteCommand;
            }
            set { _deleteCommand = value; }
        }

        private void Delete()
        {
            var list = SelectedTest;
            List<DatabaseCommand> cmdList = new List<DatabaseCommand>();
            foreach (TestSearchModel item in list)
            {
                DatabaseCommand cmd = new DatabaseCommand();
                cmd.add = null;
                cmd.delete = item.pSource;
                cmdList.Add(cmd);
            }
            if (cmdList.Any())
            {
                int userConfirmed = ViewExtension.Confirm(null, "Bạn có chắc muốn xoá những đề thi này không?");
                
                if(userConfirmed == 0)
                    return;

                ViewModelMediator.Ins.Receive(this, cmdList);
            }
        }
        #endregion

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            if (sender == this || commands.Exists((DatabaseCommand it) => it.add is TEST || it.delete is TEST))
            {
                Refresh();
            }
        }
        public void Refresh()
        {
            _test = new BindingList<TEST>((from u in DataProvider.Ins.DB.TEST select u).ToList());
            Search(Filter);
        }
    }
}
