using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class BaiChamModel
    {
        private string _idTestResult;
        public string IDTestResult { get { return _idTestResult; } set { _idTestResult = value; } }

        private string _tenMonThi;
        public string TenMonThi { get { return _tenMonThi; } set { _tenMonThi = value; } }

        private string _maLop;
        public string MaLop { get { return _maLop; } set { _maLop = value; } }

        private string _tenGV;
        public string TenGV { get { return _tenGV; } set { _tenGV = value; } }

        private string _maDeThi;
        public string MaDeThi { get { return _maDeThi; } set { _maDeThi = value; } }

        public BaiChamModel(string idTestResult, string tenMonThi, string maLop, string tenGV, string maDeThi)
        {
        }
    }
}
