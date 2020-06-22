using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    // Author: CuteTN
    class QuestionModel
    {
        private int _idQuestion;
        public int IDQuestion
        {
            get { return _idQuestion; }
            set { _idQuestion = value; }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private Nullable<int> _idDifficulty;
        public Nullable<int> IDDifficulty
        {
            get { return _idDifficulty; }
            set { _idDifficulty = value; _difficulty = loadDifficulty(); }
        }

        private string _difficulty;
        public string Difficulty
        {
            get { return _difficulty; }
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
        }

        private string loadDifficulty()
        {
            string result = DataProvider.Ins.DB.DIFFICULTY.Find(_idDifficulty).Name;
            return result;
        }

        private string loadSubject()
        {
            string result = DataProvider.Ins.DB.SUBJECT.Find(_idSubject).Name;
            return result;
        }
    }
}
