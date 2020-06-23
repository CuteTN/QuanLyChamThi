using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class TestSearchModel : INotifyPropertyChanged
    {
        #region Data
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChange("Selected"); }
        }

        private string _testDate;
        public string TestDate
        {
            get { return _testDate; }
            set { _testDate = value; OnPropertyChange("TestDate"); }
        }

        private string _testID;
        public string TestID
        {
            get { return _testID; }
            set { _testID = value; OnPropertyChange("TestID"); }
        }

        private int _testDuration;
        public int TestDuration
        {
            get { return _testDuration; }
            set { _testDuration = value; OnPropertyChange("TestDuration"); }
        }

        private TEST _pSource;
        public TEST pSource
        {
            get { return _pSource; }
            set { _pSource = value; OnPropertyChange("pSource"); }
        }
        #endregion

        #region OnPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }
}
