# Scripts/BackupMongo.ps1 - creates mongo database backup, e.g. absolute\path\to\BackupMongo.ps1 server port username password database c:\data\db\backup
# Scripts/MigrateMongo.ps1 - migrates mongo databse to lates version, e.g. absolute\path\to\MigrateMongo.ps1 server port username password database absolute\path\to\Portal.DAL.Migrations.dll
# Scripts/RestoreMongo.ps1 - resores mongo database, e.g. e.g. absolute\path\to\RestoreMongo.ps1 server port username password database c:\data\db\backup
#
# P.S. Execute from PowerShell with Set-ExecutionPolicy Unrestricted