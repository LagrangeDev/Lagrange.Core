<div align="center">

<table>
<tr>
  <td><a href="https://github.com/LagrangeDev/Lagrange.Core">Lagrange.Core</a></td>
  <td>NTQQ Protocol Implementation（👈Here</td>
</tr>
<tr>
  <td><a href="https://github.com/whitechi73/OpenShamrock">OpenShamrock</a></td>
  <td>Based on Xposed, OneBot Bot Framework</td>
</tr>
<tr>
  <td><a href="https://github.com/chrononeko/chronocat">Chronocat</a></td>
  <td>Based on Electron, modular Satori Bot Framework</td>
</tr>
</table>


# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](#)
[![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)](#)
[![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)](#)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](#)
[![Telegram](https://img.shields.io/endpoint?url=https%3A%2F%2Ftelegram-badge-4mbpu8e0fit4.runkit.sh%2F%3Furl%3Dhttps%3A%2F%2Ft.me%2F%2B6HNTeJO0JqtlNmRl)](https://t.me/+6HNTeJO0JqtlNmRl)

<a href="https://trendshift.io/repositories/3486" target="_blank"><img src="https://trendshift.io/api/badge/repositories/3486" alt="Linwenxuan05%2FLagrange.Core | Trendshift" style="width: 250px; height: 55px;" width="250" height="55"/></a>

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core

</div>

## Document
[Lagrange.Doc](https://lagrangedev.github.io/Lagrange.Doc/)

[Docker guide](Docker.md) | [Docker 使用指南](Docker_zh.md)

[English Readme](README.md) | [中文 Readme](README_zh.md)
## Out of Active Feature Requesting

Lagrange.Core has completed nearly all the function and task scheduled by Linwenxuan05, so mostly enhance to the library would be concentrated to the repo for next.

Feature request would be accepted but implemented with a long duration.

The new function of NTQQ supported in following versions would be added as well.

## Disclaimer

The Lagrange.Core project, including its developers, contributors, and affiliated individuals or entities, hereby explicitly disclaim any association with, support for, or endorsement of any form of illegal behavior. This disclaimer extends to any use or application of the Lagrange.Core project that may be contrary to local, national, or international laws, regulations, or ethical guidelines.

Lagrange.Core is an open-source software project designed to facilitate lawful and ethical applications in its intended use cases. It is the responsibility of each user to ensure that their usage of Lagrange.Core complies with all applicable laws and regulations in their jurisdiction.

The developers and contributors of Lagrange.Core assume no liability whatsoever for any actions taken by users that violate the law or engage in any form of illicit activity. Users are solely responsible for their own actions and any consequences that may arise from the use of Lagrange.Core.

Furthermore, any discussions, suggestions, or guidance provided by the Lagrange.Core community, including its developers, contributors, and users, should not be interpreted as legal advice. It is strongly recommended that users seek independent legal counsel to understand the legal implications of their actions and ensure compliance with the relevant laws and regulations.

By using or accessing Lagrange.Core, the user acknowledges and agrees to release the developers, contributors, and affiliated individuals or entities from any and all liability arising from the use or misuse of the project, including any legal consequences incurred as a result of their actions.

Please use Lagrange.Core responsibly and in accordance with the law.

### Recommend [OpenShamrock](https://github.com/whitechi73/OpenShamrock) if applicable
## Features List

| Protocol | Support | Login                     | Support | Messages  | Support | Operations        | Support | Events              | Support |
|----------|:-------:|---------------------------|:-------:|:----------|:-------:|:------------------|:-------:|:--------------------|:-------:|
| Windows  |   🟢    | QrCode                    |   🟢    | Images    |   🟢    | ~~Poke~~          |   🔴    | Captcha             |   🟢    |
| macOS    |   🟢    | Password                  |   🟢    | Text / At |   🟢    | Recall            |   🟢    | BotOnline           |   🟢    |
| Linux    |   🟢    | EasyLogin                 |   🟢    | Records   |   🟢    | Leave Group       |   🟢    | BotOffline          |   🟢    |
| Android  |   🟡    | UnusalDevice<br/>Password |   🔴    | QFace     |   🟢    | ~~Special Title~~ |   🔴    | Message             |   🟢    |
|          |         | UnusalDevice<br/>Easy     |   🟢    | Json      |   🟢    | Kick Member       |   🟢    | ~~Poke~~            |   🔴    |
|          |         | NewDeviceVerify           |   🟡    | Xml       |   🟢    | Mute Member       |   🟢    | MessageRecall       |   🟢    |
|          |         |                           |         | Forward   |   🟢    | Set Admin         |   🟢    | GroupMemberDecrease |   🟢    |
|          |         |                           |         | Video     |   🟡    | Friend Request    |   🟢    | GroupMemberIncrease |   🟢    |
|          |         |                           |         | Reply.    |   🟢    | Group Request     |   🟢    | GroupPromoteAdmin   |   🟢    |
|          |         |                           |         | File      |   🟢    | ~~Voice Call~~    |   🔴    | GroupInvite         |   🟢    |
|          |         |                           |         | Poke      |   🟢    | Client Key        |   🟢    | GroupRequestJoin    |   🟢    |
|          |         |                           |         | LightApp  |   🟢    | Cookies           |   🟢    | FriendRequest       |   🟢    |
|          |         |                           |         |           |         | Send Message      |   🟢    | ~~FriendTyping~~    |   🔴    |
|          |         |                           |         |           |         |                   |         | ~~FriendVoiceCall~~ |   🔴    |

## Lagrange.OneBot

### Implementation - OneBot v11
#### The Binary for development could be found in Actions Artifacts
<Details>
<Summary>Message Segement</Summary>

| Message Segement | Support |
|------------------|:-------:|
| [Text]           |   🟢    |
| [Face]           |   🟢    |
| [Image]          |   🟢    |
| [Record]         |   🟢    |
| [Video]          |   🟡    |
| [At]             |   🟢    |
| [Rps]            |   🟢    |
| [Dice]           |   🟢    |
| [Shake]          |   🔴    |
| [Poke]           |   🟢    |
| [Anonymous]      |   🔴    |
| [Share]          |   🔴    |
| [Contact]        |   🔴    |
| [Location]       |   🟢    |
| [Music]          |   🔴    |
| [Reply]          |   🟢    |
| [Forward]        |   🟢    |
| [Node]           |   🟢    |
| [Xml]            |   🔴    |
| [Json]           |   🟢    |

[Text]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#qq-%E8%A1%A8%E6%83%85
[Record]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E8%AF%AD%E9%9F%B3
[Face]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#qq-%E8%A1%A8%E6%83%85
[Image]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%9B%BE%E7%89%87
[Shake]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%AA%97%E5%8F%A3%E6%8A%96%E5%8A%A8%E6%88%B3%E4%B8%80%E6%88%B3-
[Poke]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%88%B3%E4%B8%80%E6%88%B3
[Anonymous]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%8C%BF%E5%90%8D%E5%8F%91%E6%B6%88%E6%81%AF-
[Location]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E4%BD%8D%E7%BD%AE
[Video]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%9F%AD%E8%A7%86%E9%A2%91
[At]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%9F%90%E4%BA%BA
[Rps]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E7%8C%9C%E6%8B%B3%E9%AD%94%E6%B3%95%E8%A1%A8%E6%83%85
[Dice]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%8E%B7%E9%AA%B0%E5%AD%90%E9%AD%94%E6%B3%95%E8%A1%A8%E6%83%85
[share]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E9%93%BE%E6%8E%A5%E5%88%86%E4%BA%AB
[Music]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E9%9F%B3%E4%B9%90%E5%88%86%E4%BA%AB-
[Contact]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E6%8E%A8%E8%8D%90%E5%A5%BD%E5%8F%8B
[Reply]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%9B%9E%E5%A4%8D
[Forward]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%90%88%E5%B9%B6%E8%BD%AC%E5%8F%91-
[Node]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#%E5%90%88%E5%B9%B6%E8%BD%AC%E5%8F%91%E8%8A%82%E7%82%B9-
[Xml]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#xml-%E6%B6%88%E6%81%AF
[Json]: https://github.com/botuniverse/onebot-11/blob/master/message/segment.md#json-%E6%B6%88%E6%81%AF

</Details>

<Details>
<Summary>API</Summary>

| API                            | Support |
|--------------------------------|:-------:|
| [/send_private_msg]            |   🟢    |
| [/send_group_msg]              |   🟢    |
| [/send_msg]                    |   🟢    |
| [/delete_msg]                  |   🟢    |
| [/get_msg]                     |   🟢    |
| [/get_forward_msg]             |   🟢    |
| [/send_like]                   |   🟢    |
| [/set_group_kick]              |   🟢    |
| [/set_group_ban]               |   🟢    |
| ~~[/set_group_anonymous_ban]~~ |   🔴    |
| [/set_group_whole_ban]         |   🟢    |
| [/set_group_admin]             |   🟢    |
| ~~[/set_group_anonymous]~~     |   🔴    |
| [/set_group_card]              |   🟢    |
| [/set_group_name]              |   🟢    |
| [/set_group_leave]             |   🟢    |
| ~~[/set_group_special_title]~~ |   🔴    |
| [/set_friend_add_request]      |   🟢    |
| [/set_group_add_request]       |   🟢    |
| [/get_login_info]              |   🟢    |
| ~~[/get_stranger_info]~~       |   🔴    |
| [/get_friend_list]             |   🟢    |
| [/get_group_info]              |   🟢    |
| [/get_group_list]              |   🟢    |
| [/get_group_member_info]       |   🟢    |
| [/get_group_member_list]       |   🟢    |
| ~~[/get_group_honor_info]~~    |   🔴    |
| [/get_cookies]                 |   🟢    |
| ~~[/get_csrf_token]~~          |   🔴    |
| ~~[/get_credentials]~~         |   🔴    |
| [/get_record]                  |   🔴    |
| [/get_image]                   |   🔴    |
| [/can_send_image]              |   🟢    |
| [/can_send_record]             |   🟢    |
| [/get_status]                  |   🔴    |
| [/get_version_info]            |   🟢    |
| [/set_restart]                 |   🟢    |
| [/clean_cache]                 |   🔴    |

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
<Summary>Event</Summary>

| PostType | EventName                      | Support |
|----------|--------------------------------|:-------:|
| Message  | [Private Message]              |   🟢    |
| Message  | [Group Message]                |   🟢    |
| Notice   | [Group File Upload]            |   🟢    |
| Notice   | [Group Admin Change]           |   🟢    |
| Notice   | [Group Member Decrease]        |   🟢    |
| Notice   | [Group Member Increase]        |   🟢    |
| Notice   | [Group Mute]                   |   🟢    |
| Notice   | [Friend Add]                   |   🟢    |
| Notice   | [Group Recall Message]         |   🟢    |
| Notice   | [Friend Recall Message]        |   🟢    |
| Notice   | [Group Poke]                   |   🔴    |
| Notice   | [Group red envelope luck king] |   🔴    |
| Notice   | [Group Member Honor Changed]   |   🔴    |
| Request  | [Add Friend Request]           |   🟢    |
| Request  | [Group Request/Invitations]    |   🟢    |
| Meta     | [LifeCycle]                    |   🟢    |
| Meta     | [Heartbeat]                    |   🟢    |

[Private Message]: https://github.com/botuniverse/onebot-11/blob/master/event/message.md#%E7%A7%81%E8%81%8A%E6%B6%88%E6%81%AF
[Group Message]: https://github.com/botuniverse/onebot-11/blob/master/event/message.md#%E7%BE%A4%E6%B6%88%E6%81%AF
[Group File Upload]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%96%87%E4%BB%B6%E4%B8%8A%E4%BC%A0
[Group Admin Change]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%AE%A1%E7%90%86%E5%91%98%E5%8F%98%E5%8A%A8
[Group Member Decrease]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E5%87%8F%E5%B0%91
[Group Member Increase]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E5%A2%9E%E5%8A%A0
[Group Mute]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%A6%81%E8%A8%80
[Friend Add]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E5%A5%BD%E5%8F%8B%E6%B7%BB%E5%8A%A0
[Group Recall Message]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%B6%88%E6%81%AF%E6%92%A4%E5%9B%9E
[Friend Recall Message]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E5%A5%BD%E5%8F%8B%E6%B6%88%E6%81%AF%E6%92%A4%E5%9B%9E
[Group Poke]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E5%86%85%E6%88%B3%E4%B8%80%E6%88%B3
[Group red envelope luck king]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E7%BA%A2%E5%8C%85%E8%BF%90%E6%B0%94%E7%8E%8B
[Group Member Honor Changed]: https://github.com/botuniverse/onebot-11/blob/master/event/notice.md#%E7%BE%A4%E6%88%90%E5%91%98%E8%8D%A3%E8%AA%89%E5%8F%98%E6%9B%B4
[Add Friend Request]: https://github.com/botuniverse/onebot-11/blob/master/event/request.md#%E5%8A%A0%E5%A5%BD%E5%8F%8B%E8%AF%B7%E6%B1%82
[Group Request/Invitations]: https://github.com/botuniverse/onebot-11/blob/master/event/request.md#%E5%8A%A0%E7%BE%A4%E8%AF%B7%E6%B1%82%E9%82%80%E8%AF%B7
[LifeCycle]: https://github.com/botuniverse/onebot-11/blob/master/event/meta.md#%E7%94%9F%E5%91%BD%E5%91%A8%E6%9C%9F
[Heartbeat]: https://github.com/botuniverse/onebot-11/blob/master/event/meta.md#%E5%BF%83%E8%B7%B3

</Details>

<Details>
<Summary>Communication</Summary>

| CommunicationType  | Support |
|--------------------|:-------:|
| [Http]             |   🟢    |
| [Http-Post]        |   🟢    |
| [ForwardWebSocket] |   🟢    |
| [ReverseWebSocket] |   🟢    |

[Http]: https://github.com/botuniverse/onebot-11/blob/master/communication/http.md
[Http-Post]: https://github.com/botuniverse/onebot-11/blob/master/communication/http-post.md
[ForwardWebSocket]: https://github.com/botuniverse/onebot-11/blob/master/communication/ws.md
[ReverseWebSocket]: https://github.com/botuniverse/onebot-11/blob/master/communication/ws-reverse.md

</Details>

#### appsettings.json Example

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

- As the Password is empty here, this indicates that QRCode login is used
- After the QRCode Login, write Uin back to perform EasyLogin

## NOTICE BEFORE LOGIN
- It is recommended that use QRCode Login for the first time as NewDeviceLogin is not implemented.
- Linux Protocol is recommended as only SignServer for such is provided
- After QRCode Login, write password and uin back to appsettings.json

## Known Problem

- ~~[ ] Signature Service is currently not established, so the login tend to be failed and return code may be 45, you can establish your own sign service by rewriting the `Signature` static class.~~

~~Thanks KonataDev/TheSnowfield for Provision of Signature API~~

Signature API is now not provided, you may need to find it somewhere and inherit `SignProvider` class for `CustomSignProvider` in  `BotConfig`

- Signature of Windows and macOS is missing, you need to figure out by your self
