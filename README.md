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

### Local - Development
*   dotnet restore
*   dotnet build
*   dotnet run

### Docker - Deployment

*   docker build -t autorenter-api-image .
*   docker run -d -p 80:3000 --name autorenter-api autorenter-api-image
*   docker attach --sig-proxy=false autorenter-api

### Docker - Cleanup

*   docker stop autorenter-api
*   docker rm autorenter-api
*   docker rmi autorenter-api-image

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Fusion Alliance for the initiative to create a community of open source development within our ranks.
