# Corporate Chat

Example chat running on docker

[![Build status](https://ci.appveyor.com/api/projects/status/45b9jhkvirhpxfep/branch/master?svg=true)](https://ci.appveyor.com/project/viniciusduartereis/corporate-chat/branch/master)
[![License](https://img.shields.io/github/license/viniciusduartereis/corporate-chat.svg)](LICENSE)

## Getting Started

### Prerequisites

* Docker
* dotnetcore
* NPM

## Running the tests on docker

Go to path `/src/Corporate.Chat.API`

```bash
docker-compose build
```

```bash
docker-compose up -d
```

Start clients

Go to path `/src/Corporate.Chat.Console.Client`

```bash
dotnet run
```

Go to path `/src/Corporate.Chat.Web.Client`

```bash
npm install && npm run dev
```

## Built With

* [docker](https://www.docker.com) - Docker
* [aspnetcore](https://docs.microsoft.com/pt-br/aspnet/core/) - Aspnet Core
* [haproxy](http://www.haproxy.org) - HAProxy
* [mssql](https://www.microsoft.com/sql-server/sql-server-2017) - MSSQL Linux (Developer License)
* [redis](https://redis.io) - Redis
* [bootstrap](https://getbootstrap.com) - Bootstrap
* [signalr](https://dotnet.microsoft.com/apps/aspnet/real-time) - SignalR
* [NPM](https://www.npmjs.com) - SignalR

## Authors

* **Vinícius Duarte Reis** - *Initial work* - [viniciusduartereis](https://github.com/viniciusduartereis)

See also the list of [contributors](https://github.com/viniciusduartereis/Corporate-Chat//contributors) who participated in this project.

## License

This project is licensed under the MIT License

## Acknowledgments

* Squad Back & Experience RV
* XP Investimentos
