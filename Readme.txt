The app consists of following folders:
- BackEnd - WebApi API - naming according to the task
- UI - React UI - naming according to the task
- Database - contains sql script db_script.sql
- TestApp.Api.Tests - contains unit tests for API (not mentioned in the task, but needed for unit tests)

To open all projects in Visual Studio just open TestApp.sln file in VS

The BackEnd:
- .NET 5, Web Api (action-based rest)
- requires to install EF Core package
- packages: EF Core
- appsettings file contains connection string to database - feel free to change it as needed to connect to your mssql
- to launch it: Ctrl+F5
- has following subfolders:
	- Controllers - web api controller
	- Models - models for web api
	- Core - domain service with business logic to lookup words
	- Dal - data access layer containing database models and db context (database-first approach used here since we must use sql to generate db, but could be easily changed to code-first if would be allowed)
- in case UI app port will be changed bear in mind to change in appsettings cors allowed host to not having issues with it

The UI:
- React, Typescript
- has been built based on default VS template for React apps. Developed changes are only in one file - words-lookup.tsx (we could also use VS Code, it's just to have everything in one solution)
- to launch it: Ctrl+F5

The Database:
- the script contains code to create MSSQL database. Feel free to comment it out if not needed
- it also creates some default words to lookup. It was not supposed that user can add new words from UI - was not mentioned in the task
- in case of errors in db just drop this db and run this script once again

Unit tests can be executed from Visual Studio. Included only main tests based on cases mentioned in the task to minimize effort for this test task 