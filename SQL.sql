/* ============================================================
   0) Xóa database cũ nếu tồn tại
   ============================================================ */
USE master;
GO
IF DB_ID(N'QuanLyProfile') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyProfile SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyProfile;
END
GO
CREATE DATABASE QuanLyProfile;
GO
USE QuanLyProfile;
GO

/* ============================================================
   1) Tạo bảng Profile
   ============================================================ */
CREATE TABLE dbo.Profile
(
    MaProfile   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenProfile  NVARCHAR(100) NOT NULL
);
GO

/* ============================================================
   2) Tạo bảng InfoProfile
   ============================================================ */
CREATE TABLE dbo.InfoProfile
(
    ID         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaProfile  INT NOT NULL,
    Gmail      NVARCHAR(150) NOT NULL,
    Pass       NVARCHAR(100) NOT NULL,
    Proxy      NVARCHAR(200) NULL,
    CONSTRAINT FK_InfoProfile_Profile FOREIGN KEY (MaProfile)
        REFERENCES dbo.Profile(MaProfile)
);
GO

/* ============================================================
   3) Thêm dữ liệu mẫu vào Profile (10 dòng)
   ============================================================ */
INSERT INTO dbo.Profile (TenProfile) VALUES
(N'Profile A'),
(N'Profile B'),
(N'Profile C'),
(N'Profile D'),
(N'Profile E'),
(N'Profile F'),
(N'Profile G'),
(N'Profile H'),
(N'Profile I'),
(N'Profile J');
GO

/* ============================================================
   4) Thêm dữ liệu mẫu vào InfoProfile (>= 10 dòng)
   ============================================================ */
INSERT INTO dbo.InfoProfile (MaProfile, Gmail, Pass, Proxy) VALUES
(1, N'user1@gmail.com',  N'Pass123!', N'192.168.1.1:8080'),
(1, N'user2@gmail.com',  N'Pass456!', N'192.168.1.2:8080'),
(2, N'user3@gmail.com',  N'Pass789!', N'192.168.1.3:8080'),
(3, N'user4@gmail.com',  N'Abc@123',  N'192.168.1.4:8080'),
(4, N'user5@gmail.com',  N'Xyz@456',  N'192.168.1.5:8080'),
(5, N'user6@gmail.com',  N'Test@789', N'192.168.1.6:8080'),
(6, N'user7@gmail.com',  N'Qwe@111',  N'192.168.1.7:8080'),
(7, N'user8@gmail.com',  N'Asd@222',  N'192.168.1.8:8080'),
(8, N'user9@gmail.com',  N'Zxc@333',  N'192.168.1.9:8080'),
(9, N'user10@gmail.com', N'Poi@444',  N'192.168.1.10:8080'),
(10, N'user11@gmail.com',N'Lkj@555',  N'192.168.1.11:8080');
GO

/* ============================================================
   5) Kiểm tra dữ liệu
   ============================================================ */
SELECT * FROM dbo.Profile;
SELECT * FROM dbo.InfoProfile;

-- Dữ liệu Profile đầy đủ
SELECT p.MaProfile, p.TenProfile, i.Gmail, i.Pass, i.Proxy
FROM dbo.Profile AS p
JOIN dbo.InfoProfile AS i ON p.MaProfile = i.MaProfile
ORDER BY p.MaProfile, i.ID;
