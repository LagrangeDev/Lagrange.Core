# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core


# Using with Docker

```bash
# 8081 port for ForwardWebSocket
docker run -d -p 8081:8081 eric1008818/lagrange.onebot:edge
```

## Persistence Storage

```bash
docker volume create lagrange_data
docker run -d -v lagrange_data:/app/data eric1008818/lagrange.onebot:edge
```

## Configure appsettings.json in Docker

### Using Mount

use bind mount to mount your `appsettings.json` to `/App/appsettings.json`		
```bash
docker run -d -v /etc/appsettings.json:/appsettings.json eric1008818/lagrange.onebot:edge
```

### Using Environment Variables

use [Enviroment Variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0#naming-of-environment-variables) to set your appsettings.json
```bash
# disable reverse websocket
docker run -d -e Implementations__1__Enable=false eric1008818/lagrange.onebot:edge
```

```bash
# input username and password
docker run -d -e Account__Uin=123456 -e Account__Password=1234 eric1008818/lagrange.onebot:edge
```