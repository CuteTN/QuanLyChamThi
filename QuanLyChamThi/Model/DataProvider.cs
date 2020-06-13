using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.Model
{
    class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new DataProvider();
                }

                return _ins;
            }

            set { _ins = value; }
        }

        public QuanLyDeThiEntities DB;
        private DataProvider()
        {
            DB = new QuanLyDeThiEntities();
        }

        public List<DETHI> DanhSachDeThi()
        {
            return DB.DETHI.Where((DETHI) =>
            // TODO: add filter
            true).ToList();
        }

        public int SoDeThi(MONHOC filterSubject)
        {
            return DB.DETHI.Where(
                (DETHI it) => it.IDSubject == filterSubject.IDSubject)
                .Count();
        }

        // Trả về danh sách tất cả các phần tử thuộc kiểu dữ liệu này
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các kiểu dữ liệu database
        public List<T> DanhSach<T>(T model) where T : class
        {
            return GetCorrespondingDbSet((dynamic)model).ToList();
        }

        // Kiểm tra phần tử này có nằm trong database không
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các dữ liệu database
        public bool KiemTraTonTai<T>(T item) where T : class
        {
            DbSet<T> dbs = GetCorrespondingDbSet((dynamic)item);
            return dbs.Any((T it) => it == item);
        }

        // Kiểm tra phần tử này có nằm trong database không
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các dữ liệu database
        // Truyền vào 1 hàm kiểm tra thay vì data.
        public bool KiemTraTonTai<T>(Func<T, bool> condition) 
            where T : class, new()
        {
            DbSet<T> dbs = GetCorrespondingDbSet((dynamic)new T());
            return dbs.Any(condition);
        }

        private DbSet<BAITHI> GetCorrespondingDbSet(BAITHI item) { return DB.BAITHI; }
        private DbSet<BAOCAONAM> GetCorrespondingDbSet(BAOCAONAM item) { return DB.BAOCAONAM; }
        private DbSet<CAUHOI> GetCorrespondingDbSet(CAUHOI item) { return DB.CAUHOI; }
        private DbSet<CHAMTHI> GetCorrespondingDbSet(CHAMTHI item) { return DB.CHAMTHI; }
        private DbSet<CHITIETBAOCAO> GetCorrespondingDbSet(CHITIETBAOCAO item) { return DB.CHITIETBAOCAO; }
        private DbSet<CHITIETDETHI> GetCorrespondingDbSet(CHITIETDETHI item) { return DB.CHITIETDETHI; }
        private DbSet<DOKHO> GetCorrespondingDbSet(DOKHO item) { return DB.DOKHO; }
        private DbSet<LOP> GetCorrespondingDbSet(LOP item) { return DB.LOP; }
        private DbSet<MONHOC> GetCorrespondingDbSet(MONHOC item) { return DB.MONHOC; }
        private DbSet<NGUOISUDUNG> GetCorrespondingDbSet(NGUOISUDUNG item){return DB.NGUOISUDUNG; }
        private DbSet<SINHVIEN> GetCorrespondingDbSet(SINHVIEN item) { return DB.SINHVIEN; }
        private DbSet<THAMSO> GetCorrespondingDbSet(THAMSO item) { return DB.THAMSO; }
        private DbSet<DETHI> GetCorrespondingDbSet(DETHI item) { return DB.DETHI; }
    }
}
