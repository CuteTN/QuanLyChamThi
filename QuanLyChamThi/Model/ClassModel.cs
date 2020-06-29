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
        string _iDClass;
        public string IDClass
        {
            get { return _iDClass; }
            set { _iDClass = value; OnPropertyChange("IDClass"); }
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
