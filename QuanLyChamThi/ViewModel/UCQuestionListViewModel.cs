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
        private ObservableCollection<Pair<int, QuestionModel>> _listQuestions;
        /// <summary>
        /// First parameter is show if that question is hightlighted or not
        /// </summary>
        public ObservableCollection<Pair<int, QuestionModel>> ListQuestions
        {
            get
            {
                if (_listQuestions == null)
                {
                    _listQuestions = new ObservableCollection<Pair<int, QuestionModel>>();
                    foreach(var question in ListQuestionModel.Ins.Data)
                    {
                        _listQuestions.Add(new Pair<int, QuestionModel>(0, question));
                    }
                }
                return _listQuestions;
            }
            set { _listQuestions = value; OnPropertyChange("ListQuestions"); }
        }
        #endregion

        private IList<QuestionModel> _selectedQuestions;
        public IList<QuestionModel> SelectedQuestions
        {
            get 
            {
                if (_selectedQuestions == null)
                    _selectedQuestions = new BindingList<QuestionModel>();
                return _selectedQuestions;
            }
            set { _selectedQuestions = value; OnPropertyChange("SelectedQuestions"); }
        }

        public void HighlightQuestions(List<QuestionModel> questionsNeedHighlight)
        {
            foreach(var question in _listQuestions)
            {
                if(questionsNeedHighlight.Find((QuestionModel param) => param.IDQuestion == question.Second.IDQuestion) != null)
                {
                    question.First = 1;
                }
            }
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // update data after adding a new question
            if(sender is QuestionViewModel)
                updateFromDB();
        }

        private void updateFromDB()
        {
            // update model from physical DB
            ListQuestionModel.Ins.UpdateFromDB();

            // this is to trigger OnPropertyChange method
            //ListQuestions = ListQuestionModel.Ins.Data;
        }
    }
}
