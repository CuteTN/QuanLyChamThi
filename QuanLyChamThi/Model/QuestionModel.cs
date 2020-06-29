using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    // Author: CuteTN
    public class QuestionModel
    {

        private Nullable<int> _idQuestion;
        public Nullable<int> IDQuestion
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
            set { /* Can't set this */ }
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

        private string loadDifficulty()
        {
            string result = DataProvider.Ins.DB.DIFFICULTY.Find(_idDifficulty)?.Name;
            return result;
        }

        private string loadSubject()
        {
            string result = DataProvider.Ins.DB.SUBJECT.Find(_idSubject)?.Name;
            return result;
        }

        // validation ////////////////////////////////////////////////////////////////////////////////////////////////////  
        static private int _maxLength = 1000;
        static public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        public enum ValidationMessage
        {
            Valid,
            InvalidSubjectID,
            InvalidDifficultyID,
            EmptyContent,
            LongContent,
        }

        public ValidationMessage Validate()
        {
            if(DataProvider.Ins.DB.SUBJECT.Find(IDSubject) == null)
                return ValidationMessage.InvalidSubjectID;
            if(DataProvider.Ins.DB.DIFFICULTY.Find(IDDifficulty) == null)
                return ValidationMessage.InvalidDifficultyID;
            if(Content == "" || Content == null)
                return ValidationMessage.EmptyContent;
            if(Content?.Length > MaxLength)
                return ValidationMessage.LongContent;
            return ValidationMessage.Valid;
        }

        // adapt to Entity Framework ////////////////////////////////////////////////////////////////////////////////////////////////////  
        
        // REFACTOR
        /// <summary>
        /// this random variable is to generate unique ID for this object
        /// </summary>
        private Random random = new Random();

        private int generateID()
        {
            int result = random.Next(); 
            
            while(DataProvider.Ins.DB.QUESTION.Find(result) != null)
                result = random.Next();

            return result;
        }

        public QUESTION AdaptDBModel()
        {
            if(Validate() != ValidationMessage.Valid)
                return null;

            QUESTION result = new QUESTION();
            result.IDQuestion = generateID();
            result.IDSubject = IDSubject;
            result.IDDifficulty = IDDifficulty.Value;
            result.Content = Content;

            return result;
        }
    }
}
