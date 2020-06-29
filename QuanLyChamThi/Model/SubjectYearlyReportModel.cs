﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class SubjectYearlyReportModel
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private string _idSubject;
        public string IDSubject
        {
            get { return _idSubject; }
            set { _idSubject = value; _subject = loadSubject(); }
        }

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set { /* Can't set this */ }
        }

        private string loadSubject()
        {
            string result = DataProvider.Ins.DB.SUBJECT.Find(_idSubject)?.Name;
            return result;
        }

        private int _testCount;
        public int TestCount
        {
            get { return _testCount; }
            set { _testCount = value; }
        }
        
        private int _testResultCount;
        public int TestResultCount
        {
            get { return _testResultCount; }
            set { _testResultCount = value; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private int _totalTestCount;
        public int TotalTestCount
        {
            get { return _totalTestCount; }
            set { _totalTestCount = value; }
        }

        // not in percent
        public double TestRatio
        {
            get
            {
                if (TotalTestCount == 0)
                    return double.NaN;
                return (double)TestCount / TotalTestCount;
            }
            set { /* Can't set this */ }
        }
        
        private int _totalTestResultCount;
        public int TotalTestResultCount
        {
            get { return _totalTestResultCount; }
            set { _totalTestResultCount = value; }
        }

        // not in percent
        public double TestResultRatio
        {
            get
            {
                if (TotalTestCount == 0)
                    return double.NaN;
                return (double)TestResultCount / TotalTestResultCount;
            }
            set { /* Can't set this */ }
        }

    }
}
