using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class ClassModel: INotifyPropertyChanged
    {
        public string IDClass;

        string _className;
        public string ClassName
        {
            get { return _className; }
            set { _className = value; OnPropertyChange("ClassName"); }
        }

        string _subjectName;
        public string SubjectName
        {
            get { _subjectName = Subject.Name; return _subjectName; }
            set { _subjectName = value; OnPropertyChange(SubjectName); }
        }

        SUBJECT _subject;
        public SUBJECT Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChange("Subject"); }
        }

        string _fullName;
        public string FullName
        {
            get
            {
                _fullName = ClassName + "_" + Year.ToString() + "_" + Semester.ToString();
                return _fullName;
            }
            set
            {
                _fullName = value;
                OnPropertyChange("FullName");
            }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChange("Year"); }
        }

        private int _semester;
        public int Semester
        {
            get { return _semester; }
            set { _semester = value; OnPropertyChange("Semester"); }
        }

        public string Username;

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
}
