using QuanLyChamThi.Model;
using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace QuanLyChamThi.ViewModel
{
    class UCQuestionListViewModel : ViewModelBase, UserModelBase
    {
        public UCQuestionListViewModel() 
        {
            // the Mediator should hold a reference to this instance to keep track of all notification
            ViewModelMediator.Ins.AddUserModel(this);    
        }

        #region DataGrid List Question
        private ObservableCollection<Pair<int, QuestionModel>> _listQuestionsView;
        /// <summary>
        /// First parameter is show if that question is hightlighted or not
        /// </summary>
        public ObservableCollection<Pair<int, QuestionModel>> ListQuestionsView
        {
            get
            {
                if (_listQuestionsView == null)
                {
                    _listQuestionsView = new ObservableCollection<Pair<int, QuestionModel>>(GetListQuestionsViewFromDB());
                }
                return _listQuestionsView;
            }
            set { _listQuestionsView = value; OnPropertyChange("ListQuestionsView"); }
        }

        private List<Pair<int, QuestionModel>> GetListQuestionsViewFromDB()
        {
            List<Pair<int, QuestionModel>> returnList = new List<Pair<int, QuestionModel>>();
            foreach (var question in ListQuestionModel.Ins.Data)
            {
                returnList.Add(new Pair<int, QuestionModel>(0, question));
            }
            return returnList;
        }
        #endregion

        #region Selected Question
        // This lise is used to display to View
        private IList<Pair<int, QuestionModel>> _selectedQuestionsView;
        public IList<Pair<int, QuestionModel>> SelectedQuestionsView
        {
            get 
            {
                if (_selectedQuestionsView == null)
                    _selectedQuestionsView = new BindingList<Pair<int, QuestionModel>>();
                SelectedQuestions.Clear();
                foreach(var questionView in _selectedQuestionsView)
                {
                    SelectedQuestions.Add(questionView.Second);
                }
                return _selectedQuestionsView;
            }
            set { _selectedQuestionsView = value; OnPropertyChange("SelectedQuestions"); }
        }

        // This list hold the selected QuestionModel
        // This list is updated synchronize with SelectedQuestionsView
        public IList<QuestionModel> SelectedQuestions = new List<QuestionModel>();
        #endregion

        #region Highlight Question
        public void HighlightQuestions(List<QuestionModel> questionsNeedHighlight)
        {
            foreach(var question in _listQuestionsView)
            {
                if(questionsNeedHighlight.Find((QuestionModel param) => param.IDQuestion == question.Second.IDQuestion) != null)
                {
                    question.First = 1;
                }
            }
        }

        public List<QuestionModel> GetHighlightedQuestions()
        {
            List<QuestionModel> highlightedQuestions = new List<QuestionModel>();
            foreach(var question in _listQuestionsView)
            {
                if(question.First == 1)
                {
                    highlightedQuestions.Add(question.Second);
                }
            }
            return highlightedQuestions;
        }
        #endregion


        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            updateFromDB();
        }

        private void updateFromDB()
        {
            // update model from physical DB
            ListQuestionModel.Ins.UpdateFromDB();

            // this is to trigger OnPropertyChange method
            ListQuestionsView = new ObservableCollection<Pair<int, QuestionModel>>(GetListQuestionsViewFromDB());
        }
    }
}
