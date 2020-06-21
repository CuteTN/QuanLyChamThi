using QuanLyChamThi.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChamThi.Model
{
    public class TestResultModel: INotifyPropertyChanged
    {
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChange("Selected"); }
        }

        private string _idTestResult;
        public string IDTestResult
        {
            get { return _idTestResult; }
            set { _idTestResult = value; }
        }

        private string _studentName;
        public string StudentName
        {
            get { return _studentName; }
            set { _studentName = value; }
        }

        private string _studentID;
        public string StudentID
        {
            get { return _studentID; }
            set { _studentID = value; }
        }

        private float? _scoreNumber;
        public float? ScoreNumber
        {
            get { return _scoreNumber; }
            set { _scoreNumber = value; }
        }

        private string _scoreString;
        public string ScoreString
        {
            get { return _scoreString; }
            set { _scoreString = value; }
        }

        private string _note;

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
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}
