using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using QuanLyChamThi.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class WindowQuestionListViewModel : ViewModelBase
    {
        readonly private List<QuestionModel> _selectedQuestions;
        public List<QuestionModel> SelectedQuestion
        {
            get
            {
                return _questionListViewModel.GetHighlightedQuestions();
            }
        }

        private UCQuestionListViewModel _questionListViewModel;

        #region Textblock xem trước câu hỏi
        private string _reviewQuestionContent;
        public string ReviewQuestionContent
        {
            get { return _reviewQuestionContent; }
            set { _reviewQuestionContent = value; OnPropertyChange("ReviewQuestionContent"); }
        }
        #endregion

        #region Double click xem nội dung câu hỏi
        private ICommand _loadSelectedQuestionContentCommand;
        public ICommand LoadSelectedQuestionContentCommand
        {
            get
            {
                if (_loadSelectedQuestionContentCommand == null)
                    _loadSelectedQuestionContentCommand = new RelayCommand(param => LoadSelectedQuestionContent());
                return _loadSelectedQuestionContentCommand;
            }
            set { _loadSelectedQuestionContentCommand = value; }
        }
        public void LoadSelectedQuestionContent()
        {
            if(_questionListViewModel.SelectedQuestions.Count != 0)
                ReviewQuestionContent = _questionListViewModel.SelectedQuestions[0].Content;
        }
        #endregion

        #region Button thêm những câu hỏi được chọn vào danh sách
        private ICommand _addSelectedQuestionCommand;
        public ICommand AddSelectedQuestionCommand
        {
            get
            {
                if (_addSelectedQuestionCommand == null)
                    _addSelectedQuestionCommand = new RelayCommand(param => AddSelectedQuestion());
                return _addSelectedQuestionCommand;
            }
            set { _addSelectedQuestionCommand = value; }
        }
        public void AddSelectedQuestion()
        {
            _questionListViewModel.HighlightQuestions(_questionListViewModel.SelectedQuestions.ToList());
        }
        #endregion

        #region Button Đồng ý
        private ICommand _acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(param => Accept());
                return _acceptCommand;
            }
            set { _acceptCommand = value; }
        }
        private void Accept()
        {
            _selectedQuestions.Clear();
            foreach(var question in _questionListViewModel.GetHighlightedQuestions())
            {
                _selectedQuestions.Add(question);
            }
        }
        #endregion

        public WindowQuestionListViewModel(List<QuestionModel> selectedQuestion, UCQuestionListViewModel questionListViewModel)
        {
            _selectedQuestions = selectedQuestion;
            _questionListViewModel = questionListViewModel;
            _questionListViewModel.HighlightQuestions(_selectedQuestions);
        }

    }
}
