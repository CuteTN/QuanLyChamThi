using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyChamThi.Model
{
    class TestModel : INotifyPropertyChanged
    {
        #region Data
        private TEST _pSource;
        public TEST pSource
        {
            get {
                TEST newitem;
                if (_pSource is null)
                {
                    newitem = new TEST();
                    newitem.DateOfTest = TestDate;
                    newitem.IDSubject = SubjectID;
                    newitem.TimeForTest = Duration;
                    newitem.Year = Year;
                    newitem.Semester = Semester;
                    if (_testID is null)
                        newitem.IDTest = SubjectID + "." + newitem.GetHashCode().ToString();
                    else
                        newitem.IDTest = _testID;
                    _pSource = newitem;
                    return _pSource;
                }
                else
                    return _pSource;
            }
            set { _pSource = value;
                OnPropertyChange("pSource");
            }
        }

        private string _testID;
        public string TestID
        {
            get { if (_testID != null)
                    return _testID;
                else return "Bài thi mới";
            }
            set {
                _testID = value;
                _pSource = null;
                OnPropertyChange("TestID");
            }
        }

        private string _subjectID;
        public string SubjectID
        {
            get { return _subjectID; }
            set {
                _subjectID = value;
                _pSource = null;
                OnPropertyChange("SubjectID");
            }
        }
        private int? _semester;
        public int? Semester
        {
            get { return _semester; }
            set {
                _semester = value;
                _pSource = null;
                OnPropertyChange("Semester"); }
        }
        private int? _year;
        public int? Year
        {
            get { return _year; }
            set {
                _year = value;
                _pSource = null;
                OnPropertyChange("Year"); }
        }
        private int _duration = 0;
        public int Duration
        {
            get { return _duration; }
            set
            {
                if (value > 0)
                {
                    _duration = value;
                    _pSource = null;
                    OnPropertyChange("Duration");
                }
            }
        }

        private DateTime? _testdate;
        public DateTime? TestDate
        {
            get { return _testdate; }
            set {_testdate = value; _pSource = null; OnPropertyChange("TestDate"); }
        }

        public TestModel Clone
        {
            get
            {
                TestModel newModel = new TestModel();
                newModel.Duration = this.Duration;
                newModel.Semester = this.Semester;
                newModel.SubjectID = this.SubjectID;
                newModel.TestDate = this.TestDate;
                newModel.TestID = this.TestID;
                newModel.Year = this.Year;
                newModel.pSource = this.pSource;
                return newModel;
            }
        }
        #endregion

        // TEST MODEL DO NOT CONTAIN A LIST OF TEST DETAIL.
        // This is just a model, not the real thing.
        #region Test Detail Model class
        public class TestDetailModel : INotifyPropertyChanged
        {
            #region Data
            private TESTDETAIL _pSource;
            public TESTDETAIL pSource
            {
                get { return _pSource; }
                set { _pSource = value; OnPropertyChange("pSource"); }
            }
            private int _stt;
            public int Stt
            {
                get { return _stt; }
                set { _stt = value; OnPropertyChange("Stt"); }
            }
            private Nullable<int> _questionID;
            public Nullable<int> QuestionID
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

            public TestDetailModel(QuestionModel that, int stt)
            {
                Content = that.Content;
                QuestionID = that.IDQuestion.Value;
                Stt = stt;
            }

            public TestDetailModel()
            {
            }
            #endregion

            #region fundamentals
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

            public TestDetailModel Clone
            {
                get
                {
                    TestDetailModel newModel = new TestDetailModel();
                    newModel.Content = this.Content;
                    newModel.QuestionID = this.QuestionID;
                    newModel.Stt = this.Stt;
                    newModel.pSource = this.pSource;
                    return newModel;
                }
            }
            #endregion
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

        public bool Valid()
        {
            return (SubjectID != null && this.Duration >= 0);
        }
    }
}
