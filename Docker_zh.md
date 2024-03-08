# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

基于 NTQQ协议 的实现，纯 C# 编写，源自 Konata.Core

# 使用Docker

```bash
# 8081 端口用于正向 WebSocket 和 Http-post
# /path-to-data 被用于存储程序运行时产生的文件
# UID Env 和 GID Env 用于设置文件权限
docker run -id -p 8081:8081 -v /path-to-data:/app/data -e UID=$UID -e GID=$(id -g) ghcr.io/lagrangedev/lagrange.onebot:edge
```

> 1. 第一次运行时会有提示 `Please Edit the appsettings.json to set configs and press any key to continue` ，请选择以下一种方式执行。
>
>    1. 修改 `/path-to-data/appsettings.json` 后重启容器
>    2. 修改 `/path-to-data/appsettings.json` 然后使用 `docker attach` 进入容器，然后按任意键，然后按 `Ctrl`+`P`，`Ctrl`+`Q` 退出容器。
>
> 2. 在 `Implementations` 里修改 `host` ，确保您想要向容器外部打开的 `Host` 被配置为 `0.0.0.0` 或者 `*`

## 从旧版本迁移

将 `appsettings.json` , `device.json` , `keystore.json` , `lagrange-*.db` 放在你想要放的目录里.  
例如 `/path-to-data`

删除 `/path-to-data/appsettings.json` 中的 `ConfigPath` 配置

按照 [使用Docker](#使用Docker) 启动容器