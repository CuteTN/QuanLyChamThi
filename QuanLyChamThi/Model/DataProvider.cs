using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using QuanLyChamThi.ViewModel;

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

        //public QuanLyDeThiEntities DB;
        private DataProvider()
        {
            //DB = new QuanLyDeThiEntities();
        }

        public List<TEST> DanhSachDeThi()
        {
            return DB.TEST.Where((DETHI) =>
            // TODO: add filter
            true).ToList();
        }

        public int SoDeThi(SUBJECT filterSubject)
        {
            //return DB.SUBJECT.Where(
            //    (TEST it) => it.IDSubject == filterSubject.IDSubject)
            //    .Count();
            return 1;
        }

        // Trả về danh sách tất cả các phần tử thuộc kiểu dữ liệu này
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các kiểu dữ liệu database
        public List<T> Get<T>() where T : class, new()
        {
            return GetCorrespondingDbSet((dynamic) new T()).ToList();
        }

        // Nhận lệnh database.
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các kiểu dữ liệu database
        // bên trong các command.
        public void Receive(List<DatabaseCommand> commands)
        {
            foreach (DatabaseCommand item in commands)
            {
                //* Remove or add a slash to toggle which code to run
                if (item.delete != null)
                    DeleteItem((dynamic)item.delete);
                if (item.add != null)
                    AddItem((dynamic)item.add);
                /*/
                if (item.add == null)
                    DeleteItem((dynamic)item.delete);
                else if (item.delete == null)
                    AddItem((dynamic)item.add);
                else
                {
                    dynamic temp = GetItem((dynamic) item.delete);
                    temp = (dynamic)item.add;
                }
                //*/
            }
            DB.SaveChanges();
            
            // TODO: Broadcast
        }

        // Xóa 1 phần tử trong database
        // Trả về thành công hay không
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các dữ liệu database
        public bool DeleteItem<T>(T item) where T : class
        {
            DbSet<T> dbs = GetCorrespondingDbSet((dynamic)item);
            return dbs.Remove(item) != null;
        }

        // Thêm item vào bảng tương ứng của database
        // Trả về thành công hay không
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các dữ liệu database
        public bool AddItem<T>(T item) where T : class
        {
            DbSet<T> dbs = GetCorrespondingDbSet((dynamic)item);
            return dbs.Add(item) != null;
        }

        // Trả về item nếu có item trong database. Cho phép sửa.
        // Tự phát hiện kiểu dữ liệu thuộc 1 trong các dữ liệu database
        public T GetItem<T>(T item) where T : class
        {
            DbSet<T> dbs = GetCorrespondingDbSet((dynamic)item);
            return dbs.Where((T it) => it == item).Single();
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

        private DbSet<TEST> GetCorrespondingDbSet(TEST item) { return DB.TEST; }
        private DbSet<CLASS> GetCorrespondingDbSet(CLASS item) { return DB.CLASS; }
        private DbSet<DIFFICULTY> GetCorrespondingDbSet(DIFFICULTY item) { return DB.DIFFICULTY; }
        private DbSet<PRINCIPLE> GetCorrespondingDbSet(PRINCIPLE item) { return DB.PRINCIPLE; }
        private DbSet<QUESTION> GetCorrespondingDbSet(QUESTION item) { return DB.QUESTION; }
        private DbSet<REPORT> GetCorrespondingDbSet(REPORT item) { return DB.REPORT; }
        private DbSet<REPORTDETAIL> GetCorrespondingDbSet(REPORTDETAIL item) { return DB.REPORTDETAIL; }
        private DbSet<STUDENT> GetCorrespondingDbSet(STUDENT item) { return DB.STUDENT; }
        private DbSet<SUBJECT> GetCorrespondingDbSet(SUBJECT item) { return DB.SUBJECT; }
        private DbSet<TESTDETAIL> GetCorrespondingDbSet(TESTDETAIL item) { return DB.TESTDETAIL; }
        private DbSet<TESTRESULT> GetCorrespondingDbSet(TESTRESULT item) { return DB.TESTRESULT; }
        private DbSet<TESTRESULTDETAIL> GetCorrespondingDbSet(TESTRESULTDETAIL item) { return DB.TESTRESULTDETAIL; }
        private DbSet<USER> GetCorrespondingDbSet(USER item) { return DB.USER; }
    }
}
