<div align="center">

![Lagrange.Core](https://socialify.git.ci/KonataDev/Lagrange.Core/image?description=1&descriptionEditable=%E4%B8%80%E4%B8%AA%E5%9F%BA%E4%BA%8E%E7%BA%AF%20C%23%20%E7%9A%84%20NTQQ%20%E5%8D%8F%E8%AE%AE%E5%AE%9E%E7%8E%B0%2C%20%E6%BA%90%E8%87%AA%20Konata.Core&font=Jost&forks=1&issues=1&logo=https%3A%2F%2Fstatic.live.moe%2Flagrange.jpg&name=1&pattern=Diagonal%20Stripes&pulls=1&stargazers=1&theme=Auto)
![Core](https://img.shields.io/badge/Lagrange-Core-blue)
![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)
![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)
![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](/LICENSE)
[![Telegram](https://img.shields.io/badge/Chat-Telegram-27A7E7)](https://t.me/+6HNTeJO0JqtlNmRl)

[![Image](https://trendshift.io/api/badge/repositories/3486)](https://trendshift.io/repositories/3486)

[English](README.md) | **&gt; 简体中文 &lt;**

</div>

## 相关项目

<table>
<tr>
  <td><a href="https://github.com/LagrangeDev/Lagrange.Core">Lagrange.Core</a></td>
  <td>NTQQ 的协议实现（👈你在这里</td>
</tr>
<tr>
  <td><a href="https://github.com/whitechi73/OpenShamrock">OpenShamrock</a></td>
  <td>基于 Lsposed 实现 OneBot 标准的机器人框架</td>
</tr>
<tr>
  <td><a href="https://github.com/chrononeko/chronocat">Chronocat</a></td>
  <td>基于 Electron 的、模块化的 Satori 框架</td>
</tr>
</table>

## 减缓新功能的实现

Lagrange.Core 已完成了几乎所有由 Linwenxuan05 安排的功能和任务，因此大部分对库的增强将集中在下一个存储库中

功能请求将被接受，但实施时间可能较长

后续版本将添加对 NTQQ 的新功能的支持

> 如果可以运行 [OpenShamrock](https://github.com/whitechi73/OpenShamrock) , 推荐使用

## 免责声明

The Lagrange.Core project, including its developers, contributors, and affiliated individuals or entities, hereby explicitly disclaim any association with, support for, or endorsement of any form of illegal behavior. This disclaimer extends to any use or application of the Lagrange.Core project that may be contrary to local, national, or international laws, regulations, or ethical guidelines.

Lagrange.Core is an open-source software project designed to facilitate lawful and ethical applications in its intended use cases. It is the responsibility of each user to ensure that their usage of Lagrange.Core complies with all applicable laws and regulations in their jurisdiction.

The developers and contributors of Lagrange.Core assume no liability whatsoever for any actions taken by users that violate the law or engage in any form of illicit activity. Users are solely responsible for their own actions and any consequences that may arise from the use of Lagrange.Core.

Furthermore, any discussions, suggestions, or guidance provided by the Lagrange.Core community, including its developers, contributors, and users, should not be interpreted as legal advice. It is strongly recommended that users seek independent legal counsel to understand the legal implications of their actions and ensure compliance with the relevant laws and regulations.

By using or accessing Lagrange.Core, the user acknowledges and agrees to release the developers, contributors, and affiliated individuals or entities from any and all liability arising from the use or misuse of the project, including any legal consequences incurred as a result of their actions.

Please use Lagrange.Core responsibly and in accordance with the law.

# Lagrange.Core

## 文档 V1

[Lagrange.Core 文档](https://lagrangedev.github.io/Lagrange.Doc/v1/Lagrange.Core/)

## 功能列表

<Details>

| 协议    | 支持情况 | 登录类型          | 支持情况 | 消息段           | 支持情况 | 操作             | 支持情况 | 事件             | 支持情况 |
| ------- | :------: | ----------------- | :------: | :--------------- | :------: | :--------------- | :------: | :--------------- | :------: |
| Windows |    🟢    | 扫码登录          |    🟢    | 图片             |    🟢    | 戳一戳       |    🟢    | 验证码           |    🟢    |
| macOS   |    🟢    | 密码登录          |    🔴    | 文本 / At        |    🟢    | 撤回消息         |    🟢    | 机器人在线       |    🟢    |
| Linux   |    🟢    | 快速登录          |    🟢    | 语音             |    🟢    | 退出群组         |    🟢    | 机器人离线       |    🟢    |
|         |          | 异常设备<br/>密码 |    🔴    | QQ 黄脸表情      |    🟢    | 特殊头衔     |    🟢    | 消息事件         |    🟢    |
|         |          | 异常设备<br/>快速 |    🟢    | Json             |    🟢    | 移除群成员       |    🟢    | 戳一戳事件   |    🟢    |
|         |          | 新设备验证        |    🔴    | Xml              |    🟢    | 禁言群成员       |    🟢    | 消息撤回事件     |    🟢    |
|         |          |                   |          | 合并转发         |    🟢    | 设置管理员       |    🟢    | 群成员减少       |    🟢    |
|         |          |                   |          | 视频             |    🟢    | 处理添加好友请求 |    🟢    | 群成员增加       |    🟢    |
|         |          |                   |          | 回复             |    🟢    | 处理加群请求     |    🟢    | 群管理员变动     |    🟢    |
|         |          |                   |          | 文件             |    🟢    | ~~语音通话~~     |    🔴    | 群组邀请         |    🟢    |
|         |          |                   |          | 窗口抖动(戳一戳) |    🟢    | Client Key       |    🟢    | 入群请求         |    🟢    |
|         |          |                   |          | 小程序           |    🟢    | Cookies          |    🟢    | 好友请求         |    🟢    |
|         |          |                   |          |                  |          | 发送消息         |    🟢    | ~~私聊输入状态~~ |    🔴    |
|         |          |                   |          |                  |          |                  |          | ~~私聊语言通话~~ |    🔴    |

</Details>

# Lagrange.OneBot

## Document

[Lagrange.OneBot 文档](https://lagrangedev.github.io/Lagrange.Doc/v1/Lagrange.OneBot/)

[Docker 使用指南](Docker_zh.md)