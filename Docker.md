# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core


# Using with Docker

**Tips:**

> Before creating the container, it is essential to mount the data folder`/root/data` to avoid the need for reauthentication every time you start the container.

> On first startup, `appsettings.json` is automatically generated and waits for any key to continue.  
> You can change `appsettings.json` and restart the container to continue.

> If you encounter network issues, you can try using `--network=host`.

```bash
# 8081 port for ForwardWebSocket
docker run -d -p 8081:8081 ghcr.io/lagrangedev/lagrange.onebot:edge
```

## Persistence Storage

```bash
docker volume create lagrange_data
docker run -d -v lagrange_data:/root/data ghcr.io/lagrangedev/lagrange.onebot:edge
```

## Configure appsettings.json in Docker

### Using Mount

use bind mount to mount your `data` folder		
```bash
docker run -d -v /path-to-data:/root/data ghcr.io/lagrangedev/lagrange.onebot:edge
```
Edit `/path-to-data/appsettings.json` with your favorite editor

### Using Environment Variables

use [Enviroment Variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0#naming-of-environment-variables) to set your appsettings.json
```bash
# disable reverse websocket
docker run -d -e Implementations__1__Enable=false ghcr.io/lagrangedev/lagrange.onebot:edge
```

```bash
# input username and password
docker run -d -e Account__Uin=123456 -e Account__Password=1234 ghcr.io/lagrangedev/lagrange.onebot:edge
```

## Migration from older versions

Move `appsettings.json`, `device.json`, `keystore.json`, `lagrange-*.db` to the same folder where you want to put them.

Use this command to start the
```bash
docker run -d -v /path-to-data:/root/data ghcr.io/lagrangedev/lagrange.onebot:edge
```
Don't forget to add your port mapping parameter, `-p`.