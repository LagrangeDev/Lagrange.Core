<div align="center">

# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![C#](https://img.shields.io/badge/.NET-%207-blue)](#)
[![License](https://img.shields.io/static/v1?label=LICENSE&message=MIT&color=lightrey)](#)

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core

</div>

## Features List
| Login                     | Support | Messages        | Support    | Operations          | Support   | Events                 | Support |
|---------------------------|---------|:----------------|:-----------|:--------------------|:----------|:-----------------------|:--------|
| QrCode                    | 🟢      | Images          | 🟡         | ~~Poke~~            | 🔴        | Captcha                | 🔴      |
| Password                  | 🔴      | Text / At       | 🟡         | Recall              | 🔴        | BotOnline              | 🟢      |
| EasyLogin                 | 🟢      | Records         | 🔴         | Leave Group         | 🔴        | BotOffline             | 🟢      |
| UnusalDevice<br/>Password | 🔴      | QFace           | 🔴         | ~~Special Title~~   | 🔴        | Message                | 🔴      |
|                           |         | Json            | 🟡         | Kick Member         | 🔴        | ~~Poke~~               | 🔴      |
|                           |         | Xml             | 🟡         | Mute Member         | 🔴        | MessageRecall          | 🔴      |
|                           |         | Forward         | 🔴         | Set Admin           | 🔴        | GroupMemberDecrease    | 🔴      |
|                           |         | Video           | 🔴         | Friend Request      | 🔴        | GroupMemberIncrease    | 🔴      |
|                           |         | ~~Flash Image~~ | 🔴         | Group Request       | 🔴        | GroupPromoteAdmin      | 🔴      |
|                           |         | Reply           | 🔴         | ~~Voice Call~~      | 🔴        | GroupInvite            | 🔴      |
|                           |         | File            | 🔴         | Csrf Token          | 🔴        | GroupRequestJoin       | 🔴      |
|                           |         |                 |            | Cookies             | 🔴        | FriendRequest          | 🔴      |
|                           |         |                 |            |                     |           | ~~FriendTyping~~       | 🔴      |
|                           |         |                 |            |                     |           | ~~FriendVoiceCall~~    | 🔴      |

## Known Problem
- [ ] Signature Service is currently not established, so the login tend to be failed and return code may be 45, you can establish your own sign service by rewriting the `Signature` static class.