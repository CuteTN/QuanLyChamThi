using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class TestResultDetailModel
    {
        string _idTestResult;
        public string IDTestResult
        {
            get { return _idTestResult; }
            set { _idTestResult = value; }
        }
        string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set { _subjectName = value; }
        }

        string _idClass;
        public string IDClass
        {
            get { return _idClass; }
            set { _idClass = value; }
        }

        string _userFullName;
        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; }
        }

        string _idTest;
        public string IDTest
        {
            get { return _idTest; }
            set { _idTest = value; }
        }

    }
}
