using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class DifficultyModel
    {
        private int _iDDifficulty;
        public int IDDifficulty
        {
            get { return _iDDifficulty; }
            set { _iDDifficulty = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DifficultyModel() { }

        public DifficultyModel(int iDDifficulty, string name)
        {
            _iDDifficulty = iDDifficulty;
            _name = name;
        }

        public DifficultyModel(DIFFICULTY difficulty)
        {
            _iDDifficulty = difficulty.IDDifficulty;
            _name = Name;
        }
    }
}
