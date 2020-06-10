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
	IDStudent varchar(256),
	HoTen varchar(256)
)
