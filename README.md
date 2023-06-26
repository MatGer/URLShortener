# URLShortener
This application needs an SQL Server in order to operate. Βelow you will find the instructions to setup and run the project yourself.

1. First you will need an SQL Server. In this file we will create a local instance of SQL server. To create the instance just open a cmd window and run this command:
   sqllocaldb create "ServerName" where ServerName (without quotes) is your server's name.
2. Next we need to connect to the server to create a database and a table. To connect we will need SMSS or a similar software. Open the program and in the Server Name field, type :"(localdb)\ServerName" (without quotes) and hit connect without adding any password.
3. We are now connected and we need to create a database. To create the database run the following query: CREATE DATABASE DbName;
4. We can finally create our Table. To create the table run the following query:
   CREATE TABLE [dbo].[Links](
	Id int IDENTITY(1,1) NOT NULL,
	ShortUrl varchar(5) NOT NULL,
	LongUrl varchar(255) NOT NULL);
!Important: Do not change the table name

6. Now we need to connect the application with the table. In Order to do this, we need to edit the appsettings.json file.
