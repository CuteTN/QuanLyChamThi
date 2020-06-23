﻿using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class TestSearchViewModel : ViewModelBase, UserModelBase
    {
        public TestSearchViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }

        #region Filter
        private class FilterState
        {
            public string TestID;
            public string TestDuration;

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
            set { _listTest = value; }
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
                    ;
                    // TODO
                    //_newTestCommand = new RelayCommand(param => MainWindowViewModel.SwitchView(7));
                return _newTestCommand;
            }
            set { _newTestCommand = value; }
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
            var list = ListTest;
            List<DatabaseCommand> cmdList = new List<DatabaseCommand>();
            foreach (TestSearchModel item in list)
                if (item.Selected)
                {
                    DatabaseCommand cmd = new DatabaseCommand();
                    cmd.add = null;
                    cmd.delete = item.pSource;
                    cmdList.Add(cmd);
                }
            if (cmdList.Any())
                ViewModelMediator.Ins.Receive(this, cmdList);
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            if (sender == this || commands.Exists((DatabaseCommand it) => it.add is TEST || it.delete is TEST))
            {
                _test = new BindingList<TEST>((from u in DataProvider.Ins.DB.TEST select u).ToList());
                Search(Filter);
            }
        }
        #endregion
    }
}
