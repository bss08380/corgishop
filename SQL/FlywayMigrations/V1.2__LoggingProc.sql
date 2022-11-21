
--AddLogEntry proc used to log entries into Logs table
--This is all configured in the nlog.config file in CorgiShop.Api

create procedure AddLogEntry (
  @machineName nvarchar(200),
  @logged datetime,
  @level varchar(5),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @callsite nvarchar(300),
  @exception nvarchar(max)
) as
begin
  insert into Logs (
    MachineName,
    Logged,
    Level,
	Message,
    Logger,
    Properties,
    Callsite,
    Exception
  ) values (
    @machineName,
    @logged,
    @level,
    @message,
    @logger,
    @properties,
    @callsite,
    @exception
  );
end