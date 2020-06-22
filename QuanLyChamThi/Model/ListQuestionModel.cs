using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    // Author: CuteTN
    class ListQuestionModel : ViewModelBase, UserModelBase
    {
        #region Data
        private ObservableCollection<QuestionModel> selectData()
        {
            ObservableCollection<QuestionModel> result = new ObservableCollection<QuestionModel>(
                    (
                        from q in DataProvider.Ins.DB.QUESTION
                        select new QuestionModel
                        {
                            IDQuestion = q.IDQuestion,
                            Content = q.Content,
                            IDSubject = q.IDSubject,
                            IDDifficulty = q.IDDifficulty,
                        }
                    ).ToList()
                );

            return result;
        }

        private ObservableCollection<QuestionModel> _data;
        public ObservableCollection<QuestionModel> Data
        {
            get
            {
                // lazy initialization
                if(_data == null)
                    _data = selectData();

                return _data;
            }
            
            set
            {
                _data = value;
            }
        }
        #endregion

        #region Singleton implementation
        // NOTE: private constructor to make this a singleton
        private ListQuestionModel() { }

        private ListQuestionModel _ins = null;
        public ListQuestionModel Ins
        {
            get
            {
                if(_ins == null)
                    _ins = new ListQuestionModel();
                return _ins;
            }
            set { _ins = value; }
        }
        #endregion

        #region Messages passing
        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            // MORECODE: some checking logic here...

            Data = selectData();

            // After update base on database, it notify all viewmodel subcribed to it
            if (collectionChanged != null)
            {
                collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// This is delagate used for notify to all other viewmodel subcribed to it when it changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void Notify(object sender, NotifyCollectionChangedEventArgs e);
        private event Notify collectionChanged;

        public void AddCollectionChangedNotified(Notify n)
        {
            // We need to seperate it because the observable collection 
            // won't call CollectionChanged when we set it
            // So we have to set it manually
            collectionChanged += n;
            Data.CollectionChanged += new NotifyCollectionChangedEventHandler(n);
        }
        #endregion
    }
}
