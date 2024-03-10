<div align="center">

<span></span>|<span></span>
-|-
[Lagrange.Core](https://github.com/LagrangeDev/Lagrange.Core)|NTQQ 的协议实现（👈你在这里
[OpenShamrock](https://github.com/whitechi73/OpenShamrock)|基于 Xposed 实现 OneBot 标准的机器人框架
[Chronocat](https://github.com/chrononeko/chronocat)|基于 Electron 的、模块化的 Satori 框架

# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

<a href="https://trendshift.io/repositories/3486" target="_blank"><img src="https://trendshift.io/api/badge/repositories/3486" alt="Linwenxuan05%2FLagrange.Core | Trendshift" style="width: 250px; height: 55px;" width="250" height="55"/></a>

基于 NTQQ协议 的实现，纯 C# 编写，源自 Konata.Core

</div>

## 文档
[Lagrange.Doc](https://lagrangedev.github.io/Lagrange.Doc/)

[Docker guide](Docker.md) | [Docker 使用指南](Docker_zh.md)

[English Readme](README.md) | [中文 Readme](README_zh.md)

## 减缓新功能的实现

Lagrange.Core 已经实现了 Linwenxuan05 计划的几乎所有功能和任务，所以接下来对库的大部分改动将集中在 repo 中。

功能请求会被接受，但需要较长时间的等待。

后续版本也会添加 NTQQ 支持的新功能。

## 免责声明

Lagrange.Core 项目，包括其开发者、贡献者以及附属个人或实体，特此明确否认与任何形式的非法行为有任何关联、支持或认可。本免责声明适用于可能违反当地、国家或国际法律、法规或道德准则的 Lagrange.Core 项目的任何使用或应用。

Lagrange.Core 是一个开源软件项目，旨在促进其预期用例中的合法和道德应用。每个用户都有责任确保其对 Lagrange.Core 的使用符合其管辖范围内的所有适用法律和法规。

Lagrange.Core 的开发者和贡献者对用户违反法律或从事任何形式非法活动的任何行为不承担任何责任。用户对自己的行为以及使用 Lagrange.Core 可能产生的任何后果承担全部责任。

此外，Lagrange.Core 社区（包括其开发人员、贡献者和用户）提供的任何讨论、建议或指导不应被解释为法律建议。强烈建议用户寻求独立的法律顾问，以了解其行为的法律影响并确保遵守相关法律法规。

通过使用或访问 Lagrange.Core，用户承认并同意免除开发者、贡献者以及关联个人或实体因使用或滥用该项目而产生的任何及所有责任，包括因其行为而产生的任何法律后果。行动。

请负责任地并依法使用 Lagrange.Core。

### 如果 [OpenShamrock] 可行, 推荐使用 [OpenShamrock]

[OpenShamrock]:(https://github.com/whitechi73/OpenShamrock) 

## 功能列表

| 协议      | 支持情况 | 登录类型                      | 支持情况 | 消息段       | 支持情况 | 操作         | 支持情况 | 事件         | 支持情况 |
|---------|:----:|---------------------------|:----:|:----------|:----:|:-----------|:----:|:-----------|:----:|
| Windows |  🟢  | 扫码登录                      |  🟢  | 图片        |  🟢  | ~~戳一戳~~    |  🔴  | 验证码        |  🟢  |
| macOS   |  🟢  | 密码登录                      |  🟢  | 文本 / At   |  🟢  | 撤回消息       |  🟢  | 机器人在线      |  🟢  |
| Linux   |  🟢  | 快速登录                      |  🟢  | 语音        |  🟢  | 退出群组       |  🟢  | 机器人离线      |  🟢  |
| Android |  🟡  | UnusalDevice<br/>Password |  🔴  | QQ 黄脸表情   |  🟢  | ~~特殊头衔~~   |  🔴  | 消息事件       |  🟢  |
|         |      | UnusalDevice<br/>Easy     |  🟢  | Json      |  🟢  | 移除群成员      |  🟢  | ~~戳一戳事件~~  |  🔴  |
|         |      | 新设备验证                     |  🟡  | Xml       |  🟢  | 禁言群成员      |  🟢  | 消息撤回事件     |  🟢  |
|         |      |                           |      | 合并转发      |  🟢  | 设置管理员      |  🟢  | 群成员减少      |  🟢  |
|         |      |                           |      | 视频        |  🟡  | 发送添加好友请求   |  🟢  | 群成员增加      |  🟢  |
|         |      |                           |      | 回复        |  🟢  | 发送加群请求     |  🟢  | 群管理员变动     |  🟢  |
|         |      |                           |      | 文件        |  🟢  | ~~语音通话~~   |  🔴  | 群组邀请       |  🟢  |
|         |      |                           |      | 窗口抖动(戳一戳) |  🟢  | Client Key |  🟢  | 入群请求       |  🟢  |
|         |      |                           |      | 小程序       |  🟢  | Cookies    |  🟢  | 好友请求       |  🟢  |
|         |      |                           |      |           |      | 发送消息       |  🟢  | ~~私聊输入状态~~ |  🔴  |
|         |      |                           |      |           |      |            |      | ~~私聊语言通话~~ |  🔴  |

## Lagrange.OneBot

### 实现 - OneBot v11
#### 二进制文件可以在 Actions 产物中找到
<Details>
<Summary>消息段</Summary>

| 消息段         | 支持情况 |
|-------------|:----:|
| [纯文本]       |  🟢  |
| [QQ 表情]     |  🟢  |
| [图片]        |  🟢  |
| [语音]        |  🟢  |
| [短视频]       |  🟡  |
| [@某人]       |  🟢  |
| [猜拳魔法表情]    |  🟢  |
| [掷骰子魔法表情]   |  🟢  |
| [窗口抖动（戳一戳）] |  🔴  |
| [戳一戳]       |  🟢  |
| [匿名发消息]     |  🔴  |
| [链接分享]      |  🔴  |
| [推荐好友]      |  🔴  |
| [位置]        |  🟢  |
| [音乐分享]      |  🔴  |
| [回复]        |  🟢  |
| [合并转发]      |  🟢  |
| [合并转发节点]    |  🟢  |
| [XML 消息]    |  🔴  |
| [JSON 消息]   |  🟢  |

[纯文本]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#qq-%E8%A1%A8%E6%83%85
[语音]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E8%AF%AD%E9%9F%B3
[QQ 表情]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#qq-%E8%A1%A8%E6%83%85
[图片]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%9B%BE%E7%89%87
[窗口抖动（戳一戳）]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%AA%97%E5%8F%A3%E6%8A%96%E5%8A%A8%E6%88%B3%E4%B8%80%E6%88%B3-
[戳一戳]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%88%B3%E4%B8%80%E6%88%B3
[匿名发消息]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%8C%BF%E5%90%8D%E5%8F%91%E6%B6%88%E6%81%AF-
[位置]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E4%BD%8D%E7%BD%AE
[短视频]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%9F%AD%E8%A7%86%E9%A2%91
[@某人]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%9F%90%E4%BA%BA
[猜拳魔法表情]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%8C%9C%E6%8B%B3%E9%AD%94%E6%B3%95%E8%A1%A8%E6%83%85
[掷骰子魔法表情]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%8E%B7%E9%AA%B0%E5%AD%90%E9%AD%94%E6%B3%95%E8%A1%A8%E6%83%85
[链接分享]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E9%93%BE%E6%8E%A5%E5%88%86%E4%BA%AB
[音乐分享]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E9%9F%B3%E4%B9%90%E5%88%86%E4%BA%AB-
[推荐好友]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%8E%A8%E8%8D%90%E5%A5%BD%E5%8F%8B
[回复]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%9B%9E%E5%A4%8D
[合并转发]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%90%88%E5%B9%B6%E8%BD%AC%E5%8F%91-
[合并转发节点]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%90%88%E5%B9%B6%E8%BD%AC%E5%8F%91%E8%8A%82%E7%82%B9-
[XML 消息]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#xml-%E6%B6%88%E6%81%AF
[JSON 消息]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#json-%E6%B6%88%E6%81%AF

</Details>

<Details>
<Summary>API</Summary>

| API                            | 支持情况 |
|--------------------------------|:----:|
| [/send_private_msg]            |  🟢  |
| [/send_group_msg]              |  🟢  |
| [/send_msg]                    |  🟢  |
| [/delete_msg]                  |  🟢  |
| [/get_msg]                     |  🟢  |
| [/get_forward_msg]             |  🟢  |
| [/send_like]                   |  🟢  |
| [/set_group_kick]              |  🟢  |
| [/set_group_ban]               |  🟢  |
| ~~[/set_group_anonymous_ban]~~ |  🔴  |
| [/set_group_whole_ban]         |  🟢  |
| [/set_group_admin]             |  🟢  |
| ~~[/set_group_anonymous]~~     |  🔴  |
| [/set_group_card]              |  🟢  |
| [/set_group_name]              |  🟢  |
| [/set_group_leave]             |  🟢  |
| ~~[/set_group_special_title]~~ |  🔴  |
| [/set_friend_add_request]      |  🟢  |
| [/set_group_add_request]       |  🟢  |
| [/get_login_info]              |  🟢  |
| ~~[/get_stranger_info]~~       |  🔴  |
| [/get_friend_list]             |  🟢  |
| [/get_group_info]              |  🟢  |
| [/get_group_list]              |  🟢  |
| [/get_group_member_info]       |  🟢  |
| [/get_group_member_list]       |  🟢  |
| ~~[/get_group_honor_info]~~    |  🔴  |
| [/get_cookies]                 |  🟢  |
| ~~[/get_csrf_token]~~          |  🔴  |
| ~~[/get_credentials]~~         |  🔴  |
| [/get_record]                  |  🔴  |
| [/get_image]                   |  🔴  |
| [/can_send_image]              |  🟢  |
| [/can_send_record]             |  🟢  |
| [/get_status]                  |  🔴  |
| [/get_version_info]            |  🟢  |
| [/set_restart]                 |  🟢  |
| [/clean_cache]                 |  🔴  |

[/send_private_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#send_private_msg-%E5%8F%91%E9%80%81%E7%A7%81%E8%81%8A%E6%B6%88%E6%81%AF
[/send_group_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#send_group_msg-%E5%8F%91%E9%80%81%E7%BE%A4%E6%B6%88%E6%81%AF 
[/send_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#send_msg-发送消息
[/delete_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#delete_msg-撤回消息
[/get_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_msg-获取消息
[/get_forward_msg]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_forward_msg-获取合并转发消息
[/send_like]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#send_like-发送好友赞
[/set_group_kick]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_kick-群组踢人
[/set_group_ban]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_ban-群组单人禁言
[/set_group_anonymous_ban]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_anonymous_ban-群组匿名用户禁言
[/set_group_whole_ban]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_whole_ban-群组全员禁言
[/set_group_admin]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_admin-群组设置管理员
[/set_group_anonymous]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_anonymous-群组匿名
[/set_group_card]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_card-设置群名片群备注
[/set_group_name]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_name-设置群名
[/set_group_leave]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_leave-退出群组
[/set_group_special_title]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_special_title-设置群组专属头衔
[/set_friend_add_request]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_friend_add_request-处理加好友请求
[/set_group_add_request]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_group_add_request-处理加群请求邀请
[/get_login_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_login_info-获取登录号信息
[/get_stranger_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_stranger_info-获取陌生人信息
[/get_friend_list]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_friend_list-获取好友列表
[/get_group_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_group_info-获取群信息
[/get_group_list]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_group_list-获取群列表
[/get_group_member_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_group_member_info-获取群成员信息
[/get_group_member_list]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_group_member_list-获取群成员列表
[/get_group_honor_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_group_honor_info-获取群荣誉信息
[/get_cookies]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_cookies-获取-cookies
[/get_csrf_token]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_csrf_token-获取-csrf-token
[/get_credentials]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_credentials-获取-qq-相关接口凭证
[/get_record]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_record-获取语音
[/get_image]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_image-获取图片
[/can_send_image]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#can_send_image-检查是否可以发送图片
[/can_send_record]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#can_send_record-检查是否可以发送语音
[/get_status]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_status-获取运行状态
[/get_version_info]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#get_version_info-获取版本信息
[/set_restart]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#set_restart-重启-onebot-实现
[/clean_cache]: https://github.com/botuniverse/onebot-11/blob/master/api/public.md#clean_cache-清理缓存

</Details>

<Details>
<Summary>事件</Summary>

| 推送类型 | 事件名       | 支持情况 |
|------|-----------|:----:|
| 消息事件 | [私聊消息]    |  🟢  |
| 消息事件 | [群消息]     |  🟢  |
| 通知事件 | [群文件上传]   |  🟢  |
| 通知事件 | [群管理员变动]  |  🟢  |
| 通知事件 | [群成员减少]   |  🟢  |
| 通知事件 | [群成员增加]   |  🟢  |
| 通知事件 | [群禁言]     |  🟢  |
| 通知事件 | [好友添加]    |  🟢  |
| 通知事件 | [群消息撤回]   |  🟢  |
| 通知事件 | [好友消息撤回]  |  🟢  |
| 通知事件 | [群内戳一戳]   |  🔴  |
| 通知事件 | [群红包运气王]  |  🔴  |
| 通知事件 | [群成员荣誉变更] |  🔴  |
| 请求事件 | [加好友请求]   |  🟢  |
| 请求事件 | [加群请求／邀请] |  🟢  |
| 元事件  | [生命周期]    |  🟢  |
| 元事件  | [心跳事件]    |  🟢  |

[私聊消息]: https://github.com/botuniverse/onebot-11/blob/master/event/message.md#%E7%A7%81%E8%81%8A%E6%B6%88%E6%81%AF
[群消息]: https://github.com/botuniverse/onebot-11/blob/master/event/message.md#%E7%BE%A4%E6%B6%88%E6%81%AF
[群文件上传]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%96%87%E4%BB%B6%E4%B8%8A%E4%BC%A0
[群管理员变动]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%AE%A1%E7%90%86%E5%91%98%E5%8F%98%E5%8A%A8
[群成员减少]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E5%87%8F%E5%B0%91
[群成员增加]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E5%A2%9E%E5%8A%A0
[群禁言]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%A6%81%E8%A8%80
[好友添加]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E5%A5%BD%E5%8F%8B%E6%B7%BB%E5%8A%A0
[群消息撤回]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%B6%88%E6%81%AF%E6%92%A4%E5%9B%9E
[好友消息撤回]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E5%A5%BD%E5%8F%8B%E6%B6%88%E6%81%AF%E6%92%A4%E5%9B%9E
[群内戳一戳]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E5%86%85%E6%88%B3%E4%B8%80%E6%88%B3
[群红包运气王]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%BA%A2%E5%8C%85%E8%BF%90%E6%B0%94%E7%8E%8B
[群成员荣誉变更]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E8%8D%A3%E8%AA%89%E5%8F%98%E6%9B%B4
[加好友请求]: https://github.com/botuniverse/onebot-11/blob/master/event/request.md#%E5%8A%A0%E5%A5%BD%E5%8F%8B%E8%AF%B7%E6%B1%82
[加群请求／邀请]: https://github.com/botuniverse/onebot-11/blob/master/event/request.md#%E5%8A%A0%E7%BE%A4%E8%AF%B7%E6%B1%82%E9%82%80%E8%AF%B7
[生命周期]: https://github.com/botuniverse/onebot-11/blob/master/event/meta.md#%E7%94%9F%E5%91%BD%E5%91%A8%E6%9C%9F
[心跳事件]: https://github.com/botuniverse/onebot-11/blob/master/event/meta.md#%E5%BF%83%E8%B7%B3

</Details>

<Details>
<Summary>通信方式</Summary>

~~哇塞全部支持~~
| 通信方式  | 支持情况 |
|--------------------|:-------:|
| [HTTP API]             |   🟢    |
| [HTTP POST]        |   🟢    |
| [正向 WebSocket] |   🟢    |
| [反向 WebSocket] |   🟢    |

[HTTP API]: https://github.com/botuniverse/onebot-11/blob/master/communication/http.md
[HTTP POST]: https://github.com/botuniverse/onebot-11/blob/master/communication/http-post.md
[正向 WebSocket]: https://github.com/botuniverse/onebot-11/blob/master/communication/ws.md
[反向 WebSocket]: https://github.com/botuniverse/onebot-11/blob/master/communication/ws-reverse.md

</Details>

#### 一个 appsettings.json 例子

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "SignServerUrl": "",
  "Account": {
    "Uin": 0,
    "Password": "",
    "Protocol": "Linux",
    "AutoReconnect": true,
    "GetOptimumServer": true
  },
  "Message": {
    "IgnoreSelf": true,
    "StringPost": false
  },
  "QrCode": {
    "ConsoleCompatibilityMode": false
  },
  "Implementations": [
    {
      "Type": "ReverseWebSocket",
      "Host": "127.0.0.1",
      "Port": 8080,
      "Suffix": "/onebot/v11/ws",
      "ReconnectInterval": 5000,
      "HeartBeatInterval": 5000,
      "AccessToken": ""
    },
    {
      "Type": "ForwardWebSocket",
      "Host": "127.0.0.1",
      "Port": 8081,
      "HeartBeatInterval": 5000,
      "AccessToken": ""
    },
    {
      "Type": "HttpPost",
      "Host": "127.0.0.1",
      "Port": 8082,
      "Suffix": "/",
      "HeartBeatInterval": 5000,
      "AccessToken": ""
    },
    {
      "Type": "Http",
      "Host": "*",
      "Port": 8083,
      "AccessToken": ""
    }
  ]
}
```

- 如果此处密码为空，则使用扫码登录
## 登录前须知
- 由于新设备登录尚未实现，建议首次使用二维码登录。
- 建议使用 Linux 协议，因为仅提供了该协议的签名服务器
- 在使用扫码登录后，将密码和 uin 写入 appsettings.json

## 已知问题

- ~~[ ] 目前尚未使用签名服务，因此容易登录失败，返回码可能为45，您可以通过重写 `Signature` 静态类以使用自己的签名服务。~~

~~感谢 KonataDev/TheSnowfield 提供的签名 API~~

暂不提供签名 API，您可能需要在某个地方找到它并在 `BotConfig` 继承 `SignProvider` 类的 `CustomSignProviderBotConfig`

- 如需使用 Windows 和 macOS 协议，你需要自行解决签名