using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using QuanLyChamThi.View;
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
            var result = (from d in DataProvider.Ins.DB.DIFFICULTY where d.Disabled == false select d).ToList();
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
                question.IDQuestion = EditingQuestion.IDQuestion.Value;
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
            var commands = CreateAddCommands();

            if (commands.Count > 0)
            {
                string msg = "Bạn có chắc muốn thêm câu hỏi mới không?";
                if(EditingQuestion != null)
                    msg = "Bạn có chắc muốn sửa câu hỏi này không?";
                int userConfirmed = ViewExtension.Confirm(null, msg);

                // user chose cancel
                if(userConfirmed == 0)
                    return;

                NotifyToMediator(commands);

                msg = "Thông báo: câu hỏi đã được thêm thành công";
                if(EditingQuestion != null)
                    msg = "Thông báo: câu hỏi đã được sửa thành công";
                ViewExtension.MessageOK(null, msg, ViewExtension.MessageType.Notification);

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

        #region Add Button Content
        private string _addButtonContent = "Thêm";
        public string AddButtonContent
        {
            get
            {
                return _addButtonContent;
            }
            set
            {
                _addButtonContent = value;
                OnPropertyChange("AddButtonContent");
            }
        }

        #endregion

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
                    if(!ListDifficulty.Contains(SelectedDifficulty))
                    {
                        ListDifficulty.Add(SelectedDifficulty);
                        OnPropertyChange("ListDifficulty");
                    }
                    SelectedSubject = editingQuestionDBModel.SUBJECT;
                }

                AddButtonContent = _editingQuestion==null? "Thêm" : "Sửa";
            }
        }
        private QuestionModel createQuestion()
        {
            var result = new QuestionModel
            {
                IDSubject = SelectedSubject?.IDSubject,
                IDDifficulty = SelectedDifficulty?.IDDifficulty,
                Content = this.Content,
            };

            return result;
        }

        private void HandleInvalidInput(QuestionModel.ValidationMessage msg)
        {
            if(msg == QuestionModel.ValidationMessage.Valid)
                return;

            string strMsg = "";

            switch(msg)
            {
                case QuestionModel.ValidationMessage.EmptyContent: strMsg = "Lỗi: không được để trống nội dung câu hỏi"; break;
                case QuestionModel.ValidationMessage.LongContent: strMsg = $"Lỗi: nội dung câu hỏi không được vượt quá {QuestionModel.MaxLength} ký tự"; break;
                case QuestionModel.ValidationMessage.InvalidDifficultyID: strMsg = "Lỗi: độ khó không hợp lệ"; break;
                case QuestionModel.ValidationMessage.InvalidSubjectID: strMsg = "Lỗi: môn học không hợp lệ"; break;
                default: throw new Exception(); 
            }

            ViewExtension.MessageOK(null, strMsg, ViewExtension.MessageType.Error);
        }

        void NotifyToMediator(List<DatabaseCommand> cmds)
        {
            ViewModelMediator.Ins.Receive(this, cmds);
        }

        private void refresh()
        {
            ListSubject = null;
            ListDifficulty = null;

        }
        #endregion

        #region Utilities
        private void resetInputContent()
        {
            Content = "";
            EditingQuestion = null;
            ListSubject = null;
            ListDifficulty = null;
            SelectedSubject = null;
            SelectedDifficulty = null;
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
            int userConfirmed = ViewExtension.Confirm(null, "Bạn có chắc muốn xoá những câu hỏi này không?");

            if(userConfirmed == 0)
                return;

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
                    _cancelEditingCommand = new RelayCommand(param => cancelEditingFunction());
                return _cancelEditingCommand;
            }
            set { _cancelEditingCommand = value; }
        }

        private void cancelEditingFunction()
        {
            if(Content != null && Content != "")
            { 
                int userConfirmed = ViewExtension.Confirm(null, "Việc huỷ sẽ xoá mọi thông tin của câu hỏi đang nhập. Tiếp tục huỷ câu hỏi?");

                if(userConfirmed == 0)
                    return;
            }

            resetInputContent();
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
            if (QuestionListViewModel.SelectedQuestions.Count != 1)
            { 
                ViewExtension.MessageOK(null, "Lỗi: vui lòng chọn 1 câu hỏi để xem", ViewExtension.MessageType.Error);
                return;
            }

            if (Content != null && Content != "" && Content != EditingQuestion?.Content)
            {
                int userConfirmed = ViewExtension.Confirm(null, "Việc tải câu hỏi khác sẽ xoá mọi thay đổi của câu hỏi đang nhập. Tiếp tục tải câu hỏi?");

                if (userConfirmed == 0)
                    return;
            }

            resetInputContent();
            EditingQuestion = QuestionListViewModel.SelectedQuestions[0];
        }
        #endregion



        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // MORECODE
            refresh();
        }

        public QuestionViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
    }
}
