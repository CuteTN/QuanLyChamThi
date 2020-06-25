using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyChamThi.Model
{
    class UCQuestionListViewModel : ViewModelBase, UserModelBase
    {
        public UCQuestionListViewModel() 
        {
            // the Mediator should hold a reference to this instance to keep track of all notification
            ViewModelMediator.Ins.AddUserModel(this);    
        }

        #region DataGrid List Question
        private ObservableCollection<QuestionModel> _listQuestion;
        public ObservableCollection<QuestionModel> ListQuestion
        {
            get
            {
                if (_listQuestion == null)
                {
                    _listQuestion = ListQuestionModel.Ins.Data;
                }
                return _listQuestion;
            }
            set { _listQuestion = value; OnPropertyChange("ListQuestion"); }
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
            ListQuestion = ListQuestionModel.Ins.Data;
        }
    }
}
