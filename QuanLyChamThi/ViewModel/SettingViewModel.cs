using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.Windows.Input;

namespace QuanLyChamThi.ViewModel
{
    class SettingViewModel: ViewModelBase
    {
        #region Difficulty

        #region Textbox Số lượng độ khó
        private int _numberOfDifficulty;
        public int NumberOfDifficulty
        {
            get { _numberOfDifficulty = ListDifficulty.Count; return _numberOfDifficulty; }
            set { _numberOfDifficulty = value; }
        }
        #endregion

        #region Button Cập nhật
        private ICommand _updateNumberDifficultyCommand;
        public ICommand UpdateNumberDifficultyCommand
        {
            get
            {
                if (_updateNumberDifficultyCommand == null)
                    _updateNumberDifficultyCommand = new RelayCommand(param => UpdateNumberDifficulty());
                return _updateNumberDifficultyCommand;
            }
            set
            {
                _updateNumberDifficultyCommand = value;
            }
        }
        private void UpdateNumberDifficulty()
        {
            // - Cảnh báo user về việc cập nhật danh sách 
            // - Các thay đổi về tên độ khó sẽ được thay đổi cho mọi câu hỏi có liên quan
            // - Các độ khó được xóa sẽ không thực sự bị xóa mà sẽ bị disable không cho hiển thị lên
            // nên các câu hỏi có sử dụng độ khó đó không bị ảnh hưởng 
            for(int i = ListDifficulty.Count; i<NumberOfDifficulty;)
            {
                if (ListDifficulty.Count < NumberOfDifficulty)
                    i++;
                else
                    i--;
            }
        }
        #endregion

        private ObservableCollection<DifficultyModel> _listDifficulty;
        public ObservableCollection<DifficultyModel> ListDifficulty
        {
            get
            {

                if (_listDifficulty == null)
                {
                    _listDifficulty = new ObservableCollection<DifficultyModel>((from d in DataProvider.Ins.DB.DIFFICULTY
                                                                                select new DifficultyModel 
                                                                                { 
                                                                                    IDDifficulty = d.IDDifficulty,
                                                                                    Name = d.Name
                                                                                }));
                }
                return _listDifficulty;
            }
            set { _listDifficulty = value; OnPropertyChange("NumberOfDifficulty"); }
        }
        #endregion

        #region Subject

        #endregion

        #region Class

        #endregion

    }
}
