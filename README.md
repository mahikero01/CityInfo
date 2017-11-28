# CityInfo
$ dotnet new projectType -> create a new project
   - webapi     ASP .NET Core Web API
   - web        ASP .NET Core empty
   
$ dotnet run -> deploy project


Dependencies fron NUGet:
$ dotnet add package Microsoft.AspNetCore.Mvc.Core --version 2.0.1   -> install asp .net mvc core dependencydotnet 

$ dotnet add package Microsoft.AspNetCore.Mvc.Formatters.Xml --version 2.0.1  -> install xml formatter 

$ dotnet add package NLog.Extensions.Logging --version 1.0.0-rtm-rc2  -> file logger

$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.0.1  -> add entity framework core

$ dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet --version 2.0.1  - add entity framework tools

To change deployment go to 
Debug -> Open configurations -> "env": {
                "ASPNETCORE_ENVIRONMENT": "Development" or "Production"}


