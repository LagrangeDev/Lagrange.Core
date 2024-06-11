<div align="center">

# Lagrange.Core - Docker guide

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

**&gt; English &lt;** | [简体中文](Docker_zh.md)

</div>

## Getting Started

```bash
# 8081 port for ForwardWebSocket and Http-Post
# /path-to-data is used to store the files needed for the runtime
# UID Env and GID Env are used to set file permissions
docker run -td -p 8081:8081 -v /path-to-data:/app/data -e UID=$UID -e GID=$(id -g) ghcr.io/lagrangedev/lagrange.onebot:edge
```

> [!IMPORTANT]
>
> 1. During the first run, you may be prompted with Please Edit the appsettings.json to set configs and press any key to continue. Please choose one of the following solutions to proceed:
>
>    1. 1. Modify `/path-to-data/appsettings.json`
>       2. Restart the container using `docker restart`
>
>    2. 1. Ensure that the `-t` option is used when executing `docker run`
>       2. Modify `/path-to-data/appsettings.json`
>       3. Enter the container using `docker attach`
>       4. Press any key
>       5. Exit the container using `Ctrl + P` `Ctrl + Q`
>
> 2. If the host needs to access the Implementation (e.g., `Http`, `ForwardWebSocket`), please configure the Host of Implementation as `*`
> 3. If the implementation needs to access the host network (e.g., `HttpPost`, `ReverseWebSocket`), please configure the `Host` of implementation to be `host.docker.internal`.

## Migration from older versions

Move `appsettings.json`, `device.json`, `keystore.json`, `lagrange-*.db` to the same folder where you want to put them.  
For example `/path-to-data`

Delete the `ConfigPath` configuration entry in `/path-to-data/appsettings.json`

Start the container according to [Getting Started](#getting-started)
