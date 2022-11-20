
--Logs table used by NLog
--Only added to by the AddLogEntry proc
--This is all configured in the nlog.config file in CorgiShop.Api

if object_id('Logs', 'U') is null
begin
	create table Logs (
		ID int not null primary key clustered (ID ASC) identity(1,1),
		MachineName nvarchar(200) null,
		Logged datetime not null,
		Level varchar(5) not null,
		Message nvarchar(max) not null,
		Logger nvarchar(300) null,
		Properties nvarchar(max) null,
		Callsite nvarchar(300) null,
		Exception nvarchar(max) null,
	);
end