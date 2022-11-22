--Create corgi API service user
--Read/write data roles only for the CorgiShop db
create login svc_corgi_api with password='fjh3_edmciv73jm';
create user svc_corgi_api for login svc_corgi_api with default_schema = CorgiShop;
alter role db_datawriter add member svc_corgi_api;
alter role db_datareader add member svc_corgi_api;

/*
Note on security
- In a real-world environment do not hard-code passwords in code/scripts (or at least change them after initial integration)
- This is NOT a real-world environment - it is used for learning/practice and simplicity of setup so I am lax with security on purpose
*/

--Products table
if object_id('Products', 'U') is null
begin
	create table Products
	(
		Id int not null primary key identity(1,1),
		Name nvarchar(128) not null,
		Description nvarchar(128) not null,
		Price decimal not null default(0.0),
		Stock int not null default(0),
		IsDeleted bit not null default(0)
	);
end

--Bogus products data for initial testing
insert into Products (Name, Description, Price, Stock) values ('Pembroke Plushy', 'An adorable Pembroke Welsh Corgi plush!', 30.0, 5);
insert into Products (Name, Description, Price, Stock) values ('Cardigan Plushy', 'An adorable Cardigan Welsh Corgi plush!', 30.0, 5);