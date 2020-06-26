using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
        public UCQuestionListViewModel QuestionListViewModel;

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
            set { _listSubject = value; OnPropertyChange("ListSubject"); }
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
            set { _selectedSubject = value; OnPropertyChange("SelectedSubject"); }
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
                if (_listDifficulty == null)
                    _listDifficulty = new BindingList<DIFFICULTY>(selectDifficulty());
                return _listDifficulty;
            }
            set { _listDifficulty = value; OnPropertyChange("ListDifficulty"); }
        }

        private DIFFICULTY _selectedDifficulty = null;
        public DIFFICULTY SelectedDifficulty
        {
            get
            {
                if (_selectedDifficulty == null)
                    if (ListDifficulty.Count > 0)
                        _selectedDifficulty = ListDifficulty[0];
                return _selectedDifficulty;
            }
            set { _selectedDifficulty = value; OnPropertyChange("SelectedDifficulty"); }
        }
        #endregion

        #region Textbox Content
        private string _content = null;
        public string Content
        {
            get
            {
                if (_content == null)
                    _content = "";
                return _content;
            }
            set { _content = value; OnPropertyChange("Content"); }
        }
        #endregion

        #region Add Button
        List<DatabaseCommand> CreateAddCommands()
        {
            List<DatabaseCommand> result = new List<DatabaseCommand>();

            // always only add 1 question
            QuestionModel questionModel = createQuestion();
            if (questionModel.Validate() != QuestionModel.ValidationMessage.Valid)
            {
                HandleInvalidInput(questionModel.Validate());
                return result;
            }
            QUESTION question = questionModel.AdaptDBModel();
            
            // if we are editting a question, we must use the same IDQuestion as EditingQuestion
            if(EditingQuestion != null)
            {
                question.IDQuestion = EditingQuestion.IDQuestion;
            }

            DatabaseCommand cmd = new DatabaseCommand
            {
                add = question,
                delete = EditingQuestion == null ? null : DataProvider.Ins.DB.QUESTION.Find(EditingQuestion.IDQuestion),
            };

            result.Add(cmd);
            return result;
        }

        void AddQuestionFunction()
        {
            // MORECODE: pop up notification

            var commands = CreateAddCommands();

            if (commands.Count > 0)
            {
                NotifyToMediator(commands);
                resetInputContent();
            }
        }

        private ICommand _addQuestionCommand = null;
        public ICommand AddQuestionCommand
        {
            get
            {
                if (_addQuestionCommand == null)
                    _addQuestionCommand = new RelayCommand(param => AddQuestionFunction());
                return _addQuestionCommand;
            }
            set { _addQuestionCommand = value; }
        }
        #endregion

        #region Internal Business logics
        private QuestionModel _editingQuestion;
        public QuestionModel EditingQuestion
        {
            get { return _editingQuestion; }
            set 
            {
                _editingQuestion = value;
                if (value != null)
                {
                    var editingQuestionDBModel = DataProvider.Ins.DB.QUESTION.Find(_editingQuestion.IDQuestion);
                    Content = editingQuestionDBModel.Content;
                    SelectedDifficulty = editingQuestionDBModel.Difficulty;
                    SelectedSubject = editingQuestionDBModel.SUBJECT;
                }
            }
        }

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

        private void HandleInvalidInput(QuestionModel.ValidationMessage msg)
        {
            MessageBox.Show(msg.ToString()); 
        }

        void NotifyToMediator(List<DatabaseCommand> cmds)
        {
            ViewModelMediator.Ins.Receive(this, cmds);
        }
        #endregion

        #region Utilities
        private void resetInputContent()
        {
            Content = "";
            EditingQuestion = null;
        }
        #endregion

        #region Button delete selected questions
        ICommand _deleteSelectedQuestionsCommand;
        public ICommand DeleteSelectedQuestionsCommand
        {
            get
            {
                if (_deleteSelectedQuestionsCommand == null)
                    _deleteSelectedQuestionsCommand = new RelayCommand(param => DeleteSelectedQuestions());
                return _deleteSelectedQuestionsCommand;
            }
            set
            {
                _deleteSelectedQuestionsCommand = value;
            }
        }

        void DeleteSelectedQuestions()
        {
            List<DatabaseCommand> commands = new List<DatabaseCommand>();
            foreach (var question in QuestionListViewModel.SelectedQuestions)
            {
                commands.Add(new DatabaseCommand
                {
                    delete = (from u in DataProvider.Ins.DB.QUESTION
                           where u.IDQuestion == question.IDQuestion
                           select u).Single(),
                    add = null
                });
            }
            ViewModelMediator.Ins.Receive(this, commands);
        }
        #endregion

        #region Button Cancel Editing
        ICommand _cancelEditingCommand;
        public ICommand CancelEditingCommand
        {
            get
            {
                if (_cancelEditingCommand == null)
                    _cancelEditingCommand = new RelayCommand(param => resetInputContent());
                return _cancelEditingCommand;
            }
            set { _cancelEditingCommand = value; }
        }
        #endregion

        #region Event double click to edit question
        ICommand _loadSelectedQuestionCommand;
        public ICommand LoadSelectedQuestionCommand
        {
            get
            {
                if (_loadSelectedQuestionCommand == null)
                    _loadSelectedQuestionCommand = new RelayCommand(param => LoadSelectedQuestion());
                return _loadSelectedQuestionCommand;
            }
            set { _loadSelectedQuestionCommand = value; }
        }

        void LoadSelectedQuestion()
        {
            EditingQuestion = QuestionListViewModel.SelectedQuestions[0];
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
