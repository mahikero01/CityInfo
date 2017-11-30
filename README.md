# CityInfo
$ dotnet new projectType -> create a new project
   - webapi     ASP .NET Core Web API
   - web        ASP .NET Core empty
   
$ dotnet run -> deploy project


Dependencies fron NUGet:
$ dotnet add package Microsoft.AspNetCore.Mvc.Core --version 2.0.1   -> install asp .net mvc core dependencydotnet 

$ dotnet add package Microsoft.AspNetCore.Mvc.Formatters.Xml --version 2.0.1  -> install xml formatter 

$ dotnet add package NLog.Extensions.Logging --version 1.0.0-rtm-rc2  -> file logger

$ dotnet add package AutoMapper --version 6.2.1     ->     OOP Mapper Boilerplate


EF Core Installation  Notes
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.0.1  -> add entity framework core

$ dotnet add package Microsoft.EntityFrameworkCore.Design  -> add entity framework tools

<DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />   -> ad in csproj file


Migration Commands

1) Create a snapshot  ->   pmc> Add-Migration CityInfoDBInitialMigration 
    $ dotnet ef migrations add NameOfTheMigration  
    e.g.  $ dotnet ef migrations add CityInfoDBInitialMigration
    
2) Update Database    ->   pmc> Update-Database
    $ dotnet ef database update
    e.g.  $ dotnet ef database update
    Or place the Database.Migrate(); in the context constructor

3) Additional changes in the Entity Classes       ->    Add-Migration CityInfoDBAddPOIDescription
    $ dotnet ef migrations add NameOfTheMigration  
    e.g.  $ dotnet ef migrations add CityInfoDBAddPOIDescription



To change deployment go to 
Debug -> Open configurations -> "env": {
                "ASPNETCORE_ENVIRONMENT": "Development" or "Production"}


