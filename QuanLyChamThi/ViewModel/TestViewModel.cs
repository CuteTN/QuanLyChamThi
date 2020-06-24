using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChamThi.ViewModel
{
    class TestViewModel : ViewModelBase, UserModelBase
    {
        #region Temporary Data
        private ObservableCollection<TestModel.TestDetailModel> _tempTestDetail;
        public ObservableCollection<TestModel.TestDetailModel> TempTestDetail
        {
            get
            {
                if (_tempTestDetail == null)
                    _tempTestDetail = new ObservableCollection<TestModel.TestDetailModel>();
                return _tempTestDetail;
            }
            set { _tempTestDetail = value; }
        }

        private TestModel _tempTest;
        public TestModel TempTest
        {
            get
            {
                if (_tempTest == null)
                    _tempTest = new TestModel();
                return _tempTest;
            }
            set { _tempTest = value; }
        }
        #endregion
        
        #region Binded Data
        private BindingList<TESTDETAIL> _testDetail;
        public BindingList<TESTDETAIL> TestDetail
        {
            get
            {
                if (_testDetail == null)
                {
                    _testDetail = new BindingList<TESTDETAIL>
                        ((from u in DataProvider.Ins.DB.TESTDETAIL where u.TEST == _test select u).ToList());
                    // _test should be initialized
                }
                return _testDetail;
            }
            set { _testDetail = value; }
        }

        private TEST _test;
        public TEST Test
        {
            get
            {
                if (_test == null) {
                    _test = new TEST();
                    // THROW EXCEPTION!
                    MessageBox.Show("This is not supposed to happen.");
                }
                return _test;
            }
            set { _test = value; }
        }
        #endregion
        
        #region View Mode: Edit or New
        public void ViewMode(string TestID)
        {
            _test = DataProvider.Ins.DB.TEST.SkipWhile((TEST it) => it.IDTest != TestID).First();
        }
        #endregion

        #region Button Accept

        #endregion

        #region Button Cancel

        #endregion

        #region Button MoveUp
        #endregion

        #region Button MoveDown
        #endregion

        #region Basic View Model info
        public TestViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
