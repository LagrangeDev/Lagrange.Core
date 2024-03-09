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
# 8081 port for ForwardWebSocket and Http-Post
# /path-to-data is used to store the files needed for the runtime
# UID Env and GID Env are used to set file permissions
docker run -id -p 8081:8081 -v /path-to-data:/app/data -e UID=$UID -e GID=$(id -g) ghcr.io/lagrangedev/lagrange.onebot:edge
```

> 1. The first time you run it, you will be prompted `Please Edit the appsettings.json to set configs and press any key to continue` Please choose one of the following methods to execute.
>
>    1. Restart the container after modifying `/path-to-data/appsettings.json`
>    2. Modify `/path-to-data/appsettings.json` and use `docker attach` to enter the container and press any key, then use `Ctrl`+`P`; `Ctrl`+`Q` to exit the container.
>
> 2. Make sure that the `Host` in the `Implementations` that you want to open to outside the container is configured as `0.0.0.0` or `*`.

## Migration from older versions

Move `appsettings.json`, `device.json`, `keystore.json`, `lagrange-*.db` to the same folder where you want to put them.  
For example `/path-to-data`

Delete the `ConfigPath` configuration entry in `/path-to-data/appsettings.json`

Start the container according to [Using with Docker](#using-with-docker)