using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class TestResultModel
    {
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

        private int? _scoreNumber;
        public int? ScoreNumber
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
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}
