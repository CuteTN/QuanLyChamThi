using QuanLyChamThi.Command;
using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class WindowQuestionListViewModel: ViewModelBase
    {
        private List<QuestionModel> _selectedQuestions;
        private UCQuestionListViewModel _questionListViewModel;

        private string _reviewQuestionContent;
        public string ReviewQuestionContent
        {
            get { return _reviewQuestionContent; }
            set { _reviewQuestionContent = value; OnPropertyChange("ReviewQuestionContent"); }
        }

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

        public WindowQuestionListViewModel(List<QuestionModel> selectedQuestion, UCQuestionListViewModel questionListViewModel)
        {
            _selectedQuestions = selectedQuestion;
            _questionListViewModel = questionListViewModel;
        }

    }
}
