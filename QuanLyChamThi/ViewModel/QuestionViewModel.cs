using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class QuestionViewModel : ViewModelBase, UserModelBase
    {

        #region Combobox Subject
        private List<SUBJECT> selectSubject()
        {
            var result = (from s in DataProvider.Ins.DB.SUBJECT select s).ToList();
            return result;
        }

        private BindingList<SUBJECT> _listSubject = null;
        public BindingList<SUBJECT> ListSubject
        {
            get
            {
                if (_listSubject == null)
                    _listSubject = new BindingList<SUBJECT>(selectSubject());
                return _listSubject;
            }
            set { _listSubject = value; }
        }

        private SUBJECT _selectedSubject = null;
        public SUBJECT SelectedSubject
        {
            get
            {
                if (_selectedSubject == null)
                    if (ListSubject.Count > 0)
                        _selectedSubject = ListSubject[0];
                return _selectedSubject;
            }
            set { _selectedSubject = value; }
        }
        #endregion

        #region Combobox Difficulty
        private List<DIFFICULTY> selectDifficulty()
        {
            var result = (from d in DataProvider.Ins.DB.DIFFICULTY select d).ToList();
            return result;
        }
        
        private BindingList<DIFFICULTY> _listDifficulty = null;
        public BindingList<DIFFICULTY> ListDifficulty
        {
            get
            {
                if(_listDifficulty == null)
                    _listDifficulty = new BindingList<DIFFICULTY>(selectDifficulty());
                return _listDifficulty;
            }
            set { _listDifficulty = value; }
        }

        private DIFFICULTY _selectedDifficulty = null;
        public DIFFICULTY SelectedDifficulty
        {
            get
            {
                if(_selectedDifficulty == null)
                    if(ListDifficulty.Count > 0)
                        _selectedDifficulty = ListDifficulty[0];
                return _selectedDifficulty;
            }
            set { _selectedDifficulty = value; }
        }
        #endregion

        #region Textbox Content
        private string _content = null;
        public string Content
        {
            get
            {
                if(_content == null)
                    _content = "";
                return _content;
            }
            set { _content = value; }
        }
        #endregion

        #region Add Button
        void AddQuestionFunction()
        {
            var commands = CreateAddCommands();
            NotifyToMediator(commands);
        }

        private ICommand _addQuestionCommand = null;
        public ICommand AddQuestionCommand
        {
            get
            {
                if(_addQuestionCommand == null)
                    _addQuestionCommand = new RelayCommand(param => AddQuestionFunction());
                return _addQuestionCommand;
            }
            set { _addQuestionCommand = value; }
        }
        #endregion

        #region Internal Business logics
        private QuestionModel editingQuestion = null;

        private QuestionModel createQuestion()
        {
            var result = new QuestionModel
            {
                IDSubject = SelectedSubject.IDSubject,
                IDDifficulty = SelectedDifficulty.IDDifficulty,
                Content = this.Content,
            };

            return result;
        }

        List<DatabaseCommand> CreateAddCommands()
        {
            List<DatabaseCommand> result = new List<DatabaseCommand>();

            // always only add 1 question
            QuestionModel questionModel = createQuestion();
            if(questionModel.Validate() != QuestionModel.ValidationMessage.Valid)
            { 
                HandleInvalidInput(questionModel.Validate());
                return result;
            }
            QUESTION question = questionModel.AdaptDBModel();

            DatabaseCommand cmd = new DatabaseCommand
            {
                add = question, 
                delete = editingQuestion,
            };

            result.Add(cmd);
            return result;
        }

        private void HandleInvalidInput(QuestionModel.ValidationMessage msg)
        {
            MessageBox.Show(msg.ToString()); 
        }

        void NotifyToMediator(List<DatabaseCommand> cmds)
        {
            ViewModelMediator.Ins.Receive(this, cmds);
        }
        #endregion


        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // MORECODE
        }

        public QuestionViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
    }
}
