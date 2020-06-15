
create table DOKHO
(
	IDDifficulty int primary key,
	TenDoKho nvarchar(256),
)

create table MONHOC
(
	IDSubject nvarchar(20) primary key,
	TenMonHoc nvarchar(256),
)

create table CAUHOI
(
	IDQuestion int primary key,
	NoiDung nvarchar(max),
	IDDifficulty int foreign key references DOKHO(IDDifficulty),
	IDSubject nvarchar(20) foreign key references MONHOC(IDSubject)
)

create table DETHI
(
	IDTest nvarchar(256) primary key,
	IDSubject nvarchar(20) foreign key references MONHOC(IDSubject),
	NamHoc int,
	HocKi int,
	ThoiLuong int,
	NgayThi smalldatetime
)

create table CHITIETDETHI
(
	IDTest nvarchar(256) foreign key references DETHI(IDTest),
	Stt int,
	IDQuestion int foreign key references CAUHOI(IDQuestion),
	constraint PK_CHITIETDETHI primary key (IDTest, Stt)
)


create table NGUOISUDUNG
(
	Username nvarchar(256) primary key,
	LoaiNguoiDung int,
	MatKhau nvarchar(max),
	HoTen nvarchar(256)
)

create table LOP
(
	IDClass nvarchar(256) primary key,
	IDSubject nvarchar(20) foreign key references MONHOC(IDSubject),
	Username nvarchar(256) foreign key references NGUOISUDUNG(Username),
	HocKi int,
	Nam int
)

create table SINHVIEN
(
	IDStudent nvarchar(256) primary key,
	HoTen nvarchar(256)
)

create table CHAMTHI
(
	IDTestResult nvarchar(256) primary key,
	IDClass nvarchar(256) foreign key references LOP(IDClass),
	Username nvarchar(256) foreign key references NGUOISUDUNG(Username),
	IDTest nvarchar(256) foreign key references DETHI(IDTest),
	GhiChu nvarchar(256)
)

create table BAITHI
(
	IDTestResult nvarchar(256) foreign key references CHAMTHI(IDTestResult),
	IDStudent nvarchar(256) foreign key references SINHVIEN(IDStudent),
	DiemSo int,
	DiemChu nvarchar(256),
	GhiChu nvarchar(256),
	constraint PK_BAITHI primary key (IDTestResult, IDStudent)
)

create table BAOCAONAM
(
	Nam int primary key,
	TongSoDeThi int,
	TongSoBaiCham int
)

create table CHITIETBAOCAO
(
	IDSubject nvarchar(20) foreign key references MONHOC(IDSubject),
	Nam int foreign key references BAOCAONAM(Nam),
	SoLuongDeThi int,
	SoLuongBaiCham int,
	constraint PK_CHITIETBAOCAO primary key (IDSubject, Nam)
)

create table THAMSO
(
	id int primary key,
	SoCauToiDa int,
	ThoiLuongToiDa int,
	ThoiLuongToiThieu int,
	DiemSoToiDa int,
	DiemSoToiThieu int
)