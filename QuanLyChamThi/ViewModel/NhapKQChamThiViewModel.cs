using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyChamThi.Model;
using QuanLyChamThi.Command;
using System.ComponentModel;

namespace QuanLyChamThi.ViewModel
{
    public class pair<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public pair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    class NhapKQChamThiViewModel: ViewModelBase
    {
        private ObservableCollection<pair<bool, BaiCham>> _dsBaiCham = new ObservableCollection<pair<bool, BaiCham>>();
        public ObservableCollection<pair<bool, BaiCham>> DSBaiCham
        {
            get { return _dsBaiCham; }
            set
            {
                _dsBaiCham = value;
                OnPropertyChange("DSBaiCham");
            }
        }

        #region ComboBox Giảng viên chấm bài
        private BindingList<string> _dsGV;
        public BindingList<string> DSGV
        {
            get
            {
                if(_dsGV == null)
                {
                    _dsGV = new BindingList<string>();
                }
                return _dsGV;
            }

            set { _dsGV = value; }
        }

        private string _selectedGV;
        public string SelectedGV
        {
            get { return _selectedGV; }
            set
            {
                _selectedGV = value;
                OnPropertyChange("SelectedGV");
            }
        }
        #endregion

        #region ComboBox Môn Thi
        private BindingList<string> _dsMon;
        public BindingList<string> DSMon
        {
            get
            {
                if (_dsMon == null)
                {
                    _dsMon = new BindingList<string>();
                }
                return _dsMon;
            }

            set { _dsMon = value; }
        }

        private string _selectedMon;
        public string SelectedMon
        {
            get { return _selectedMon; }
            set
            {
                _selectedMon = value;
                OnPropertyChange("SelectedMon");
            }
        }
        #endregion

        #region ComboBox Lớp
        private BindingList<string> _dsLop;
        public BindingList<string> DSLop
        {
            get
            {
                if (_dsLop == null)
                {
                    _dsLop = new BindingList<string>();
                }
                return _dsLop;
            }

            set { _dsLop = value; }
        }

        private string _selectedLop;
        public string SelectedLop
        {
            get { return _selectedLop; }
            set
            {
                _selectedLop = value;
                OnPropertyChange("SelectedLop");
            }
        }
        #endregion

        #region ComboBox Mã đề thi
        private BindingList<string> _dsMaDeThi;
        public BindingList<string> DSMaDeThi
        {
            get
            {
                if (_dsMaDeThi == null)
                {
                    _dsMaDeThi = new BindingList<string>();
                }
                return _dsMaDeThi;
            }

            set { _dsMaDeThi = value; }
        }

        private string _selectedMaDeThi;
        public string SelectedMaDeThi
        {
            get { return _selectedMaDeThi; }
            set
            {
                _selectedMaDeThi = value;
                OnPropertyChange("SelectedMaDeThi");
            }
        }
        #endregion

        public NhapKQChamThiViewModel()
        {
            DSGV.Add("Hồ Công Thành");
            DSGV.Add("Phan Nguyễn Anh Đào");

            DSMon.Add("IT001");
            DSMon.Add("IT002");
            DSMon.Add("IT003");

            DSLop.Add("IT001.K11");
            DSLop.Add("IT001.K12");
            DSLop.Add("IT001.K13");
            DSLop.Add("IT001.K14");
            DSLop.Add("IT002.K11");
            DSLop.Add("IT002.K12");
            DSLop.Add("IT002.K13");
            DSLop.Add("IT003.K11");
            DSLop.Add("IT003.K12");

            DSMaDeThi.Add("IT001-001");
            DSMaDeThi.Add("IT001-002");
            DSMaDeThi.Add("IT002-001");
            DSMaDeThi.Add("IT002-002");
            DSMaDeThi.Add("IT002-003");
            DSMaDeThi.Add("IT003-001");
        }

        private ICommand _themBaiCham;
        public ICommand cmdThemBaiCham
        {
            get
            {
                if (_themBaiCham == null)
                {
                    _themBaiCham = new RelayCommand(param => ThemBaiCham());
                }
                return _themBaiCham;
            }
            set
            {
                _themBaiCham = value;
            }
        }
        private void ThemBaiCham()
        {
            DSBaiCham.Add(new pair<bool, BaiCham>(false, new BaiCham()));
        }

        private ICommand _xoaBaiChamDuocChon;
        public ICommand cmdXoaBaiChamDuocChon
        {
            get
            {
                if (_xoaBaiChamDuocChon == null) 
                    _xoaBaiChamDuocChon = new RelayCommand(param => XoaBaiChamDuocChon());
                return _xoaBaiChamDuocChon;
            }
            set { _xoaBaiChamDuocChon = value; }
        }
        private void XoaBaiChamDuocChon()
        {
            for(int i=0; i<_dsBaiCham.Count; )
            {
                if (_dsBaiCham[i].Item1 == true)
                    _dsBaiCham.RemoveAt(i);
                else
                    i++;
            }
        }

        private ICommand _chonTatCa;
        public ICommand cmdChonTatCa
        {
            get
            {
                if (_chonTatCa == null)
                    _chonTatCa = new RelayCommand(param => ChonTatCaBaiCham());
                return _chonTatCa;
            }
            set { _chonTatCa = value; }
        }
        private void ChonTatCaBaiCham()
        {

        }

        private ICommand _luu;
        public ICommand cmdLuu
        {
            get
            {
                if (_luu == null)
                    _luu = new RelayCommand(param => LuuBaiCham());
                return _luu;
            }
            set { _luu = value; }
        }
        private void LuuBaiCham()
        {

        }

        private ICommand _huy;
        public ICommand cmdHuy
        {
            get
            {
                if (_huy == null)
                    _huy = new RelayCommand(param => HuyBaiCham());
                return _huy;
            }
            set { _huy = value; }
        }
        private void HuyBaiCham()
        {

        }
    }
}
