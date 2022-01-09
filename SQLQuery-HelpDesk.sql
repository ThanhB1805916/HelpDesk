create database  HelpDesk
go

use HelpDesk
go

create table Users(
    id_user int IDENTITY(1,1) primary key,
    username varchar(50) not null,
    pwd varchar(100) not null,
    name varchar(50),
    rightt int /* 0: mannage, 1: Tech, 2: Staff*/
)
go

create table Category(
    id_Category int IDENTITY(1,1) primary key,
    nameCategory varchar(255) default 'category'
)
go

create table Device(
    id_Device int IDENTITY(1,1) primary key,
    id_Category int,
    nameDevice varchar(255) default 'device',
	id_User int
)
go

create table FAQ(
    id_FAQ int IDENTITY(1,1) primary key,
    question nvarchar(255),
    answer nvarchar(255),
    rightt int default 0, /*0: private, 1: public, -1: suggest*/
	id_User int default -1, /*nguoi them FAQ*/
	countt int default 0/*số lần sử dụng FAQ*/
)
go

create table Trouble(
    id_Trouble int IDENTITY(1,1) primary key,
	id_Report int default -1, /*id nhân viên báo cáo*/
    id_Fill int default -1, /*id của người điền báo cáo*/
    /*id_Tech int default -1,*/
    id_Manage int default -1,
    id_Device int default -1,
	/*id_FAQ int default -1,*/
    describe nvarchar(255),
	images nvarchar(255),
    status int default -1, /*trạng thái: 
								0: sent - đã gửi, chờ manage đánh giá phân quyền
								1: received - manager giao cho tech xử lí
								2: troubleshooting - tech nhận troubl và xử lý
								3: finished - xử lí hoàn tất*/
    level int default -1, /* mức độ nghiêm trọng: 0: ít nghiêm trọng, 1: nghiêm trọng, 2: rất nghiêm trọng*/  
    dateStaff date default getDate(),
    dateManage date,
    dateTech date,
	
)
go

/*create table DetailDevice(
	id_DeDev int IDENTITY(1,1) primary key,
	id_Device int,
	id_Trouble int,
	id_Bill nvarchar(254),
	content nvarchar(254)
)*/

/*create table DetailFAQ(
	id_DetailFAQ int IDENTITY(1,1) primary key,
	id_Trouble int not null,
	id_FAQ int not null,
	describe nvarchar(254),
	finishTime int
)
go*/

create table DetailTech(
	id_DetailTech int IDENTITY(1,1) primary key,
	id_Trouble int not null,
	id_Tech int not null,
	status int default -1, /*trạng thái: 
								0: received - manager giao cho tech xử lí
								1: troubleshooting - tech nhận troubl và xử lý
								2: sendback
								3: finished*/
	/*deadline date,*/
	dateFinish date,
	describe nvarchar(254), /*manager mo ta cong viec cho tech*/
	fixMethod int, /*0: sua binh thuong
					 1: thay moi phan cung*/	
	id_Bill nvarchar(254),
	contentBill nvarchar(254),
	/*detail faq*/
	id_FAQ int,
	describeFAQ nvarchar(254),
	finishTime int
)
go

create table languages(
	id nvarchar(2),
	name nvarchar(254),
	isdefault bit
)

insert into languages values('vi',N'Việt Nam',0)
insert into languages values('en','English',1)

/*Users*/
insert into users (username, pwd, rightt) values ('admin', '159357', 0);
insert into users (username, pwd, rightt) values ('tech', '1111', 0);
insert into users (username, pwd, rightt) values ('staff', '2222', 0);
/*Category*/
insert into Category (nameCategory) values ('Screen')
insert into Category (nameCategory) values ('Printer')
insert into Category (nameCategory) values ('Keyboard')
insert into Category (nameCategory) values ('Camera')

/*Device*/
insert into Device (id_Category, nameDevice, id_User) values (1, 'Dell L456', 3)
insert into Device (id_Category, nameDevice, id_User) values (1, 'Asus M123',5)
insert into Device (id_Category, nameDevice, id_User) values (2, 'LG PE47',3)

/*FAQ*/
insert into FAQ (question, answer, rightt) values ('Why is my code not run', 'Because i false', '0')
insert into FAQ (question, answer, rightt) values ('How to qua mon nay', 'finish this project', '1')

/*alter Database HelpDesk set  Enable_Broker*/

ALTER TABLE DetailTech
ADD deadline date