# AutoRenter - ASP.NET Core

An ASP.NET Core based implementation of the AutoRenter API.

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

- Download and run the [.NET Core 1.0 RC4 SDK Installer](https://github.com/dotnet/core/blob/master/release-notes/rc4-download.md).

## How To

- Unless otherwise noted, all terminal commands must be issued from the project's root directory.

### Local Development

1. Open the project folder in your favorite text editor (preferably VS Code)
1. Run `dotnet restore` from the command line
   1. Optionally run `dotnet build` if you prefer to build before running. Running will also build.
1. Run `dotnet run` from the command line

### Browse the app

After successfully starting the API app, you should be able to view data by browsing to [http://127.0.0.1:3000/api/locations](http://127.0.0.1:3000/api/locations).
For more in-depth testing, use a web debugging tool such as [Fiddler](https://www.telerik.com/download/fiddler) or [Postman](https://www.getpostman.com/).

[Postman collection](https://www.getpostman.com/collections/5530fbffa46505020891)

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

The supported version is 1.0.0-rc4-*.

### The project does not load in Visual Studio

The [Microsoft Documentation](https://www.microsoft.com/net/core#windowsvs2015) says that this will work with Visual Studio Update 3. Not so. The project file format has been changed from a JSON-based format to an XML-based format (allegedly to maintain compatibility with MSBuild). However, VS Update 3 does not yet support this new format.

## Contributing

Please read the [CONTRIBUTING](./CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Fusion Alliance for the initiative to create a community of open source development within our ranks.
