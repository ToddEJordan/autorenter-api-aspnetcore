[![Build Status](https://travis-ci.org/fusionalliance/autorenter-api-aspnetcore.svg?branch=development)](https://travis-ci.org/fusionalliance/autorenter-api-aspnetcore)

# AutoRenter - ASP.NET Core

An ASP.NET Core based implementation of the AutoRenter API.

## Overview

These instructions will cover usage information for the API and the optional development virtual machine.

## Frameworks - Packages - Patterns - Features used

- ASP.NET Core
- Entity Framework Core
- FluentValidation
- AutoMapper
- Global exception handler
- Cors
- xUnit
- Moq

## Prerequisites

- Download and install [.NET Core 1.0](https://dot.net/core)

## How To

- Unless otherwise noted, all terminal commands must be issued from the solution's root directory.
- See the [Azure Deployment Guide](https://github.com/fusionalliance/autorenter-api-aspnetcore/blob/development/azure-deployment.md) for information on deploying the project to Azure.

### Local Development

1. Open the project folder in your favorite text editor (preferably VS Code).
2. Run `dotnet restore` from the command line to restore dependencies.
3. Run `dotnet build` to build the solution.
   a. Note: running will also build.
4. To run api tests, navigate to a test folder (e.g., `./AutoRenter.Api.Tests`) and run `dotnet test'.
5. To run all api tests in Bash run './RunTests.sh'.
6. Ensure the ASPNETCORE_ENVIRONMENT variable is set to development. On Windows: `set ASPNETCORE_ENVIRONMENT=Development`. On Unix based OS: `export ASPNETCORE_ENVIRONMENT=Development`.
6. To run the app, navigate to the `AutoRenter.Api` folder and run `dotnet run`.


### Browse the app

After successfully starting the API app, you should be able to view data by browsing to [http://127.0.0.1:3000/api/locations](http://127.0.0.1:3000/api/locations).
For more in-depth testing, use a web debugging tool such as [Fiddler](https://www.telerik.com/download/fiddler) or [Postman](https://www.getpostman.com/).

### API Route Documentation

Once running locally, you can access API route documentation (useful for those consuming the API) by going to: [http://localhost:3000/docs/api/](http://localhost:3000/docs/api/)

### Docker Development

Note: If you want to use Docker, you will need to point your browser (and Postman, if you use it) to 192.168.99.100:3000 instead of 127.0.0.1:3000.

To build and start the app:
```bash
docker build -t autorenter-api-image .
docker run -d -p 3000:3000 --name autorenter-api autorenter-api-image
```

To connect to the container:
```bash
docker attach --sig-proxy=false autorenter-api
```

To clean up the container once your done with it:
```bash
docker stop autorenter-api
docker rm autorenter-api
docker rmi autorenter-api-image
```

## Troubleshooting

### The app does not run

Make sure you have the correct version of ASP.NET Core:

```bash
dotnet --version
```

The supported version is 1.0.4

### The project does not load in Visual Studio

The [Microsoft Documentation](https://www.microsoft.com/net/core#windowsvs2015) says that this will work with Visual Studio Update 3. Not so. The project file format has been changed from a JSON-based format to an XML-based format (allegedly to maintain compatibility with MSBuild). However, VS Update 3 does not yet support this new format.
Consider using [Visual Studio 2017](https://www.microsoft.com/net/core#windowsvs2017) or [Visual Studio Code](https://code.visualstudio.com/download).

## Contributing

Please read the [CONTRIBUTING](./CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Fusion Alliance for the initiative to create a community of open source development within our ranks.
