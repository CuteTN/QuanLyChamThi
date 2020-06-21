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

namespace QuanLyChamThi.ViewModel
{
    class TestResultViewModel
    {
        ICollectionView _listTestResultDetail;
        public ICollectionView ListTestResultDetail
        {
            get
            {
                if (_listTestResultDetail == null)
                {
                    _listTestResultDetail = CollectionViewSource.GetDefaultView(ListTestResultDetailModel.Ins.Data);
                    _listTestResultDetail.Filter = Filter;
                    _listTestResultDetail.Refresh();
                }
                return _listTestResultDetail;
            }
            set { _listTestResultDetail = value; }
        }

        private bool Filter(object item)
        {
            TestResultDetailModel test = item as TestResultDetailModel;

            ///////// PUT FILER HERE ///////////////
            return test.SubjectName == "Nhập môn lập trình";
        }

        ICommand _command;
        public ICommand Command
        {
            get
            {
                if (_command == null)
                    _command = new RelayCommand(param => CMD());
                return _command;
            }

            set { _command = value; }
        }

        void CMD()
        {
            var x = ListTestResultDetailModel.Ins.Data;
        }

        public TestResultViewModel()
        {
        }
    }
}
