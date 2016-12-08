# AutoRenter - ASP.NET Core 1.1

An ASP.NET Core 1.1 based implementation of the AutoRenter API.

## Overview

These instructions will cover usage information for the API and the optional development virtual machine.

## Frameworks - Packages - Patterns - Features used

- ASP.NET Core
- Entity Framework Core
- Repository Pattern
- Automapper
- Global exception handler
- Cors

## Prerequisites

- Install [.NET Core](https://www.microsoft.com/net/core).

### Local - Development - With Visual Studio

1. Open the project in VS 2015 or higher
2. Build and run

### Local - Development - Without Visual Studio

1. Open the project folder in your favorite text editor (preferable VS Code)
2. Run `dotnet restore` from the command line
   1. Optionally run `dotnet build` if you prefer to build before running. Running will also build.
3. Run `dotnet run` from the command line

### Docker - Deployment (or Development)

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
