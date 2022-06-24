Drop database PTPM_QLBanHang
CREATE DATABASE PTPM_QLBanHang
USE PTPM_QLBanHang
set dateformat dmy
create table NhaSanXuat
(
	MaNSX int not null primary key identity,
	TenNSX nvarchar(255),
	DiaChi nvarchar(255),
	SDT varchar(12)
)
create table LoaiSanPham
(
	MaLoaiSP int primary key identity,
	TenLoaiSP nvarchar(MAX),
)
create table SanPham
(
	MaSP int not null primary key identity,
	TenSP nvarchar(255),
	HinhAnh nvarchar(MAX),
	DonGia decimal(18,0),
	SoLuongTon int,
	DaBan int,
	MaNSX int,
	MaLoaiSP int,
	foreign key (MaNSX) references NhaSanXuat(MaNSX) on delete cascade,
	foreign key (MaLoaiSP) references LoaiSanPham(MaLoaiSP),
)
create table NhanVien
(
	MaNV nvarchar(30) primary key not null,
	HoTen nvarchar(100),
	DiaChi nvarchar(255),
	NgaySinh datetime,
	ChucVu nvarchar(50),
	Luong int,
	SoDienThoai nvarchar(20),
)
create table TaiKhoan
(
	MaTK int identity primary key not null,
	TK nvarchar(50),
	MK varchar(50),
	MaNV nvarchar(30),
	Quyen nvarchar(10),
	TrangThai nvarchar(10),
	foreign key (MaNV) references NhanVien(MaNV) on delete cascade
)
create table KhachHang
(
	MaKH int identity primary key not null,
	Hoten nvarchar(255),
	DiaChi nvarchar(255),
	NgaySinh datetime,
	SoDienThoai nvarchar(20),
)
create table PhieuNhap
(
	MaPN int not null primary key identity,
	MaNSX int,
	NgayNhap datetime,
	foreign key (MaNSX) references NhaSanXuat(MaNSX) on delete cascade,
)
create table ChiTietPhieuNhap
(
	MaChiTietPN int not null primary key identity,
	MaPN int,
	MaSP int,
	DonGiaNhap decimal(18,0),
	SoLuongNhap int,
	foreign key (MaPN) references PhieuNhap(MaPN) on delete cascade,
	foreign key (MaSP) references SanPham(MaSP)
)
create table HoaDon
(
	MaHD nvarchar(10) primary key not null,
	NgayBan datetime,
	MaKH int,
	MaNV nvarchar(30),
	TongTien decimal(18,0),
	foreign key (MaKH) references KhachHang(MaKH) on delete cascade,
	foreign key (MaNV) references NhanVien(MaNV) on delete cascade,
)
create table ChiTietHoaDon
(
	MaCTHD int identity primary key not null,
	MaHD nvarchar(10),
	MaSP int,
	TenSP nvarchar(255),
	SoLuong int,
	DonGia decimal(18,0),
	foreign key (MaHD) references HoaDon(MaHD) on delete cascade,
	foreign key (MaSP) references SanPham(MaSP) on delete cascade,
)



insert into NhanVien values('DA01',N'Hà Quán Hưng',N'Quận 10','1/1/2001',N'Nhân viên','1800000','09123456789')
insert into NhanVien values('DA02',N'Lương Minh Thành',N'Quận 8','1/2/2001',N'Nhân viên','5000000','09987654321')

insert into TaiKhoan values('hungha','309201','DA01','User','Offline')
insert into TaiKhoan values('thanhluong','285201','DA02','User','Offline')
insert into TaiKhoan values('admin','123',null,'Admin','Offline')

insert into KhachHang values(N'Nguyễn Tràn Hoàng Hưng',N'TPHCM','28/2/2001','09147852369')
insert into KhachHang values(N'Trần Hoàng Hưng',N'Hà Nội','31/3/2000','09159357846')
insert into KhachHang values(N'Nguyễn Hoàng Hưng',N'Bến Tre','31/12/2002','09963258741')

insert into NhaSanXuat values(N'Công ty cung cấp quần áo sỉ lẻ XXX',N'123/456/789 Sư Vạn Hạnh Q10','09111222333')

insert into LoaiSanPham values(N'Áo thun')
insert into LoaiSanPham values(N'Áo sơ mi')
insert into LoaiSanPham values(N'Quần jeans')
insert into LoaiSanPham values(N'Quần short')
insert into LoaiSanPham values(N'Nón')

insert into SanPham values(N'Áo thun kẻ sọc mát mẻ',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\ao_thun_ke_soc.png',450000,100,80,1,1)
insert into SanPham values(N'Áo thun cổ tròn ngắn tay',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\ao_thun_co_tron_ngan_tay.png',200000,200,10,1,1)
insert into SanPham values(N'Áo sơ mi cổ co giãn công sở',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\ao_so_mi_co_co_gian_cong_so.png',600000,150,140,1,2)
insert into SanPham values(N'Áo sơ mi ngắn tay',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\ao_so_mi_ngan_tay.png',300000,50,30,1,2)
insert into SanPham values(N'Quần jeans siêu co giãn',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\quan_jean_sieu_co_gian.png',900000,300,300,1,3)
insert into SanPham values(N'Quần jeans ống suông',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\quan_jean_ong_suong.png',999000,100,50,1,3)
insert into SanPham values(N'Quần joggers',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\quan_joggers.png',14000,100,22,1,3)
insert into SanPham values(N'Quần short leo núi',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\quan_short_leo_nui.png',780000,100,90,1,4)
insert into SanPham values(N'Quần short siêu nhẹ',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\quan_short_sieu_nhe.png',400000,300,200,1,4)
insert into SanPham values(N'Nón NewYork',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\non_newyork.png',100000,100,20,1,5)
insert into SanPham values(N'Nón lưỡi trai',N'D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\non_luoi_trai.png',150000,100,50,1,5)

insert into HoaDon values('HD01','17/03/2021',1,'DA02',900000)
insert into HoaDon values('HD02','11/07/2021',2,'DA01',1200000)
insert into HoaDon values('HD03','12/07/2021',1,'DA01',300000)

insert into ChiTietHoaDon values('HD01',1,N'Áo thun kẻ sọc mát mẻ',2,450000)
insert into ChiTietHoaDon values('HD02',4,N'Quần short siêu nhẹ',3,400000)
insert into ChiTietHoaDon values('HD03',2,N'Áo sơ mi cổ co giãn công sở',1,300000)