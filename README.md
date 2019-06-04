# Corporate Chat

Example chat running on docker

## Getting Started

### Prerequisites

* Docker
* dotnetcore
* NPM

## Running the tests on docker

Go to path /src/Corporate.Chat.API

```bash
docker-compose build
```

```bash
docker-compose up -d
```

Start clients

Go to path /src/Corporate.Chat.Console.Client

```bash
dotnet run
```

Go to path /src/Corporate.Chat.Web.Client

```bash
npm run dev
```

## Built With

* [docker](https://www.docker.com) - docker
* [aspnetcore](https://docs.microsoft.com/pt-br/aspnet/core/) - aspnet core
* [haproxy](http://www.haproxy.org) - haproxy
* [mssql](https://www.microsoft.com/sql-server/sql-server-2017) - mssql linux (Developer License)
* [redis](https://redis.io) - redis
* [bootstrap](https://getbootstrap.com) - bootstrap
* [signalr](https://dotnet.microsoft.com/apps/aspnet/real-time) - signalr

## Authors

* **Vinícius Duarte Reis** - *Initial work* - [PurpleBooth](https://github.com/viniciusduartereis)

See also the list of [contributors](https://github.com/viniciusduartereis/Corporate-Chat//contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Squad Back & Experience RV
* XP Investimentos
