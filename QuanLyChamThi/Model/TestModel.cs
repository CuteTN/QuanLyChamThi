using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class TestModel : INotifyPropertyChanged
    {
        #region Data
        private string _testID;
        public string TestID
        {
            get { return _testID; }
            set { _testID = value; OnPropertyChange("TestID"); }
        }

        private string _subjectID;
        public string SubjectID
        {
            get { return _subjectID; }
            set { _subjectID = value; OnPropertyChange("SubjectID"); }
        }
        private string _semester;
        public string Semester
        {
            get { return _semester; }
            set { _semester = value; OnPropertyChange("Semester"); }
        }
        private string _year;
        public string Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChange("Year"); }
        }
        #endregion

        // TEST MODEL DO NOT CONTAIN A LIST OF TEST DETAIL.
        // This is just a model, not the real thing.
        #region Test Detail Model class
        public class TestDetailModel : INotifyPropertyChanged
        {
            private string _stt;
            public string Stt
            {
                get { return _stt; }
                set { _stt = value; OnPropertyChange("Stt"); }
            }
            private string _questionID;
            public string QuestionID
            {
                get { return _questionID; }
                set { _questionID = value; OnPropertyChange("QuestionID"); }
            }
            private string _content;
            public string Content
            {
                get { return _content; }
                set { _content = value; OnPropertyChange("Content"); }
            }

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
        }
        #endregion

        #region Event
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
