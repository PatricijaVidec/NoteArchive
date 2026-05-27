## **WELCOME TO NOTE ARCHIVE**

## Overview
Note Archive is a web application built with **.NET 8 MVC** that allows users to securely archive, manage, and restore digital copies of their music sheets.

The purpose of the application is to ensure that important sheet music is never permanently lost.  
If a physical copy becomes damaged, destroyed, or misplaced, users can simply download the archived file, and print it again.


## Features

- User authentication and authorization
- Upload and archive music sheets
- Download archived sheets
- File organization and management
- Search and filtering functionality
- File metadata management
- Print-ready document downloads

## Technologies Used

- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Docker**
- **Bootstrap 5**
- **Razor Views**
- **Identity Authentication**

# **DOWNLOAD**
1. Clone the repository
```bash
git clone https://github.com/PatricijaVidec/NoteArchive
```
2. Navigate to the repository
```bash
cd NoteArchive
```
3. Make sure you run a docker container, you can download it [here](https://www.docker.com/products/docker-desktop/)
4. then to run the docker container you run this code (Windows)
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-CU15-GDR1-ubuntu-22.04
```
5. Then you must update the database and run the aplication
```bash
dotnet ef database update
dotnet run
```
5. Open the localhost link it provides
