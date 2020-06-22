﻿using System;
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
                _listTestResultDetailView = new ObservableCollection<TestResultDetailModel>(ListTestResultDetailModel.Ins.Data.Where((TestResultDetailModel param) => param.SubjectName == "Nhập môn lập trình"));
                return _listTestResultDetailView;
            }
            set
            {
                _listTestResultDetailView = value;
            }
        }

        ICommand _test;
        public ICommand Test
        {
            get
            {
                if (_test == null)
                    _test = new RelayCommand(p => TestFunc());
                return _test;
            }
            set
            {
                _test = value;
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

        void TestFunc()
        {
            ListTestResultDetailModel.Ins.Data.Remove(SelectedResultDetail);
            DataProvider.Ins.DB.TESTRESULTDETAIL.Remove(DataProvider.Ins.DB.TESTRESULTDETAIL.Where((TESTRESULTDETAIL param) => param.IDTestResult == _selectedResultDetail.IDTestResult).Single());
            DataProvider.Ins.DB.SaveChanges();
        }

        private bool Filter(object item)
        {
            TestResultDetailModel test = item as TestResultDetailModel;

            ///////// PUT FILER HERE ///////////////
            return test.SubjectName == "Nhập môn lập trình";
        }

        void test2(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChange("ListTestResultDetailView");
            MessageBox.Show("");
        }

        public TestResultDetailedViewModel()
        {
            ListTestResultDetailModel.Ins.AddCollectionChangedNotified(test2);
        }
    }
}
