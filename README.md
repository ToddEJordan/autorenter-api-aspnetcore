# AutoRenter - ASP.NET Core 1.1

An ASP.NET Core 1.1 based implementation of the AutoRenter API.

## Overview

These instructions will cover usage information for the API and the optional development virtual machine.

## Frameworks - Packages - Patterns - Features used

- ASP.NET Core
- Entity Framework Core
- Repository Pattern
- Automapper
- FluentValidation
- CQS Pattern - MediatR
- Global exception handler
- Cors

## Prerequisites

- Install [.NET Core](https://www.microsoft.com/net/download/core#/current).
  Be sure to select the .NET Core 1.1 SDK Installer option

### Local - Development - With Visual Studio

1. Open the project in VS 2015 or higher
2. Build and run

### Local - Development - Without Visual Studio

1. Open the project folder in your favorite text editor (preferable VS Code)
2. Run `dotnet restore` from the command line
   1. Optionally run `dotnet build` if you prefer to build before running. Running will also build.
3. Run `dotnet run` from the command line

### Browse the app

After successfully starting the API app, you should be able to view data by browsing to [http://127.0.0.1:3000/api/locations](http://127.0.0.1:3000/api/locations).
For more in-depth testing, use a web debugging tool such as [Fiddler](https://www.telerik.com/download/fiddler) or [Postman](https://www.getpostman.com/).

[Postman collection](https://www.getpostman.com/collections/5530fbffa46505020891)

### Docker - Deployment (or Development)

Note: If you want to try this with docker, postman needs to be updated to point to 192.168.99.100:3000 instead of 127.0.0.1:3000.

In a command line, run the following form the project's root:
```bash
docker build -t autorenter-api-image .
docker run -d -p 3000:3000 --name autorenter-api autorenter-api-image
```

To connect to the container:
```bash
docker attach --sig-proxy=false autorenter-api
```

### Docker - Cleanup

To clean up the container once your done with it:
```bash
docker stop autorenter-api
docker rm autorenter-api
docker rmi autorenter-api-image
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Fusion Alliance for the initiative to create a community of open source development within our ranks.
