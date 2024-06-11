<div align="center">

# Lagrange.Core - Docker 使用指南

一个基于纯 C# 的 NTQQ 协议实现，源自 Konata.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

[English](Docker.md) | **&gt; 简体中文 &lt;**

</div>

## 开始使用

```bash
# 8081 端口用于正向 WebSocket 和 Http-post
# /path-to-data 被用于存储程序运行时产生的文件
# UID Env 和 GID Env 用于设置文件权限
docker run -td -p 8081:8081 -v /path-to-data:/app/data -e UID=$UID -e GID=$(id -g) ghcr.io/lagrangedev/lagrange.onebot:edge
```

> [!IMPORTANT]
>
> - 首次运行时可能会提示 `Please Edit the appsettings.json to set configs and press any key to continue`，请选择以下一种方案执行：
>
>   - 1.  修改 `/path-to-data/appsettings.json`
>     2.  使用 `docker restart` 重新启动容器
>
>   - 1.  确保在执行 `docker run` 时使用了 `-t` 选项
>     2.  修改 `/path-to-data/appsettings.json`
>     3.  使用 `docker attach` 进入容器
>     4.  按任意键
>     5.  使用 `Ctrl + P` `Ctrl + Q` 退出容器
>
> - 如果需要宿主需要访问实现（例如：`Http`，`ForwardWebSocket`），请将实现的 `Host` 配置为 `*`
> - 如果实现需要访问宿主网络（例如：`HttpPost`，`ReverseWebSocket`），请将实现的 `Host` 配置为 `host.docker.internal`

## 从旧版本迁移

将 `appsettings.json`、`device.json`、`keystore.json`、`lagrange-*.db` 移动到您想要放置它们的相同文件夹中。
例如 `/path-to-data`

删除 `/path-to-data/appsettings.json` 中的 `ConfigPath` 配置

按照 [# 开始使用](#开始使用) 启动容器
