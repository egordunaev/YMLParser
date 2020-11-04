# YML Parser

This project was made as a test project for parsing YML info into dbs. Please, **do not use this** project.
## Before you run
Before running this project you have to build and run docker container with sql database.
To build container:
```
docker build -t mssql:dev .
```
This build creates mssql container and creates db and table inside it. See more at `creaete-db.sql`.

To run container:
```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password1!' -e 'MSSQL_PID=Express' --name sqlserver -p 1433:1433 -d mssql:dev
```
## Running
To run this project use these examples:

To parse yml from `url` and store `shopId` data use:
```
dotnet run -- save <shopId> <url>
```

To show last 50 entries of `shopId` (in CSV format) use:
```
dotnet run -- print <shopId>
```

## Examples
Parsing and saving data:
```
dotnet run -- save 11111 http://static.ozone.ru/multimedia/yml/facet/mobile_catalog/1133677.xml
dotnet run -- save 2 http://static.ozone.ru/multimedia/yml/facet/div_soft.xml
dotnet run -- save 3 http://static.ozone.ru/multimedia/yml/facet/business.xml
```

Printing stored data in csv format:
```
dotnet run -- print 11111
dotnet run -- print 2
dotnet run -- print 3
```