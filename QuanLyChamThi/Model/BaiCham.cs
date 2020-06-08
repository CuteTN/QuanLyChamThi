using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    public class BaiCham
    {
        private string _mssv;
        public string MSSV
        {
            get { return _mssv; }
            set 
            { 
                _mssv = value; 
            }
        }

        private string _tenSV;
        public string TenSV
        {
            get { return _tenSV; }
            set 
            {
                _tenSV = value;
            }
        }

        private int? _diemSo;
        public int? DiemSo
        {
            get { return _diemSo; }
            set { _diemSo = value; }
        }

        private string _diemChu;
        public string DiemChu
        {
            get { return _diemChu; }
            set { _diemChu = value; }
        }

        private string _ghiChu;
        public string GhiChu
        {
            get { return _ghiChu; }
            set { _ghiChu = value; }
        }

        public BaiCham(string mssv, string tenSV, int diemSo, string diemChu, string ghiChu)
        {
            _mssv = mssv;
            _tenSV = tenSV;
            _diemSo = diemSo;
            _diemChu = diemChu;
            _ghiChu = ghiChu;
        }

        public BaiCham()
        {
            _mssv = "";
            _tenSV = "";
            _diemSo = null;
            _diemChu = "";
            _ghiChu = "";
        }
    }
}
