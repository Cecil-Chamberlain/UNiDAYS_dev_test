# UNiDAYS_dev_test
Web form to add a new user to an SQL database.

# Build database
In the terminal, to open SQL prompt (where 'SQLEXPRESS' is the name of the local instance):
```
sqlcmd -S .\SQLEXPRESS
```
Then regular SQL code:
```
>>> CREATE DATABASE UNiDAYS;
>>> go
>>> USE UNiDAYS;
>>> go
>>> CREATE TABLE Users ( UserID int NOT NULL IDENTITY(1,1) PRIMARY KEY, UserEmail varchar(255) NOT NULL UNIQUE, UserPassword varchar(255) NOT NULL, UserSalt varchar(255) NOT NULL);
>>> go
```

# Encryption of connection string
Encrypt the connection string in web.config.
```
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
aspnet_regiis -pef connectionStrings C:\path\to\location\of\web.config
```
Decrypt the connection string in web.config.
```
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
aspnet_regiis -pdf connectionStrings C:\path\to\location\of\web.config
```
Crucially, the path should only identify the folder which holds web.config rather than point to the file its self.
