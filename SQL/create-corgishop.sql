--The only thing done here is create the CorgiShop database
--The database-specific flyway migration scripts in /FlywayMigrations take it from there and set up the various db objects
--The corgi shop API service user is also set up in the initial migration script
if not exists(select * from sys.databases where name = 'CorgiShop')
begin
	create database CorgiShop;
end