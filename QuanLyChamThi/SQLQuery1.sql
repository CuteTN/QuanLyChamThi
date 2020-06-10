create database QuanLyDeThi
go

use QuanLyDeThi
go

create table DOKHO
(
	IDDifficulty int primary key,
	TenDoKho varchar(256),
)

create table MONHOC
(
	IDSubject varchar(20) primary key,
	TenMonHoc varchar(256),
)

create table CAUHOI
(
	IDQuestion int primary key,
	NoiDung varchar(5000),
	IDDifficulty int foreign key references DOKHO(IDDifficulty),
	IDSubject varchar(20) foreign key references MONHOC(IDSubject)
)

create table DETHI
(
	IDTest varchar(256) primary key,
	IDSubject varchar(20) foreign key references MONHOC(IDSubject),
	NamHoc int,
	HocKi int,
	ThoiLuong int,
	NgayThi smalldatetime
)

create table CHITIETDETHI
(
	IDTest varchar(256) foreign key references DETHI(IDTest),
	Stt int,
	IDQuestion int foreign key references CAUHOI(IDQuestion),
	constraint PK_CHITIETDETHI primary key (IDTest, Stt)
)


create table NGUOISUDUNG
(
	Username varchar(256) primary key,
	LoaiNguoiDung int,
	MatKhau varchar(max),
	HoTen varchar(256)
)

create table LOP
(
	IDClass varchar(256) primary key,
	IDSubject varchar(20) foreign key references MONHOC(IDSubject),
	Username varchar(256) foreign key references NGUOISUDUNG(Username),
	HocKi int,
	Nam int
)

create table SINHVIEN
(
	IDStudent varchar(256) primary key,
	HoTen varchar(256)
)

create table CHAMTHI
(
	IDTestResult varchar(256) primary key,
	IDClass varchar(256) foreign key references LOP(IDClass),
	Username varchar(256) foreign key references NGUOISUDUNG(Username),
	IDTest varchar(256) foreign key references DETHI(IDTest),
	GhiChu varchar(256)
)

create table BAITHI
(
	IDTestResult varchar(256) foreign key references CHAMTHI(IDTestResult),
	IDStudent varchar(256) foreign key references SINHVIEN(IDStudent),
	DiemSo int,
	DiemChu varchar(256),
	GhiChu varchar(256),
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
	IDSubject varchar(20) foreign key references MONHOC(IDSubject),
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