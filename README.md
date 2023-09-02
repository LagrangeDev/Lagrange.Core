<div align="center">

# Lagrange.Core

[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](#)
[![C#](https://img.shields.io/badge/.NET-%207-blue)](#)
[![License](https://img.shields.io/static/v1?label=LICENSE&message=MIT&color=lightrey)](#)

An Implementation of NTQQ Protocol, with Pure C#, Derived from Konata.Core

</div>

## Disclaimer:

The Lagrange.Core project, including its developers, contributors, and affiliated individuals or entities, hereby explicitly disclaim any association with, support for, or endorsement of any form of illegal behavior. This disclaimer extends to any use or application of the Lagrange.Core project that may be contrary to local, national, or international laws, regulations, or ethical guidelines.

Lagrange.Core is an open-source software project designed to facilitate lawful and ethical applications in its intended use cases. It is the responsibility of each user to ensure that their usage of Lagrange.Core complies with all applicable laws and regulations in their jurisdiction.

The developers and contributors of Lagrange.Core assume no liability whatsoever for any actions taken by users that violate the law or engage in any form of illicit activity. Users are solely responsible for their own actions and any consequences that may arise from the use of Lagrange.Core.

Furthermore, any discussions, suggestions, or guidance provided by the Lagrange.Core community, including its developers, contributors, and users, should not be interpreted as legal advice. It is strongly recommended that users seek independent legal counsel to understand the legal implications of their actions and ensure compliance with the relevant laws and regulations.

By using or accessing Lagrange.Core, the user acknowledges and agrees to release the developers, contributors, and affiliated individuals or entities from any and all liability arising from the use or misuse of the project, including any legal consequences incurred as a result of their actions.

Please use Lagrange.Core responsibly and in accordance with the law.

## Features List
| Protocol | Support | Login                     | Support | Messages         | Support    | Operations        | Support    | Events                 | Support |
|----------|---------|---------------------------|---------|:-----------------|:-----------|:------------------|:-----------|:-----------------------|:--------|
| Windows  | 🟢      | QrCode                    | 🟢      | Images           | 🟢         | ~~Poke~~          | 🔴         | Captcha                | 🟢      |
| macOS    | 🔴      | Password                  | 🟢      | Text / At        | 🟢         | Recall            | 🟡         | BotOnline              | 🟢      |
| Linux    | 🟢      | EasyLogin                 | 🟢      | ~~Records~~      | 🔴         | Leave Group       | 🔴         | BotOffline             | 🟢      |
|          |         | UnusalDevice<br/>Password | 🔴      | QFace            | 🟢         | ~~Special Title~~ | 🔴         | Message                | 🟢      |
|          |         | UnusalDevice<br/>Easy     | 🟢      | Json             | 🟡         | Kick Member       | 🟢         | ~~Poke~~               | 🔴      |
|          |         | NewDeviceVerify           | 🔴      | Xml              | 🟢         | Mute Member       | 🟢         | MessageRecall          | 🔴      |
|          |         |                           |         | Forward          | 🟢         | Set Admin         | 🟢         | GroupMemberDecrease    | 🟢      |
|          |         |                           |         | Video            | 🔴         | Friend Request    | 🔴         | GroupMemberIncrease    | 🟢      |
|          |         |                           |         | ~~Flash Image~~  | 🔴         | Group Request     | 🔴         | GroupPromoteAdmin      | 🟢      |
|          |         |                           |         | Reply            | 🟢         | ~~Voice Call~~    | 🔴         | GroupInvite            | 🟢      |
|          |         |                           |         | File             | 🟡         | Client Key        | 🟢         | GroupRequestJoin       | 🔴      |
|          |         |                           |         |                  |            | Cookies           | 🟢         | FriendRequest          | 🔴      |
|          |         |                           |         |                  |            | Send Message      | 🟢         | ~~FriendTyping~~       | 🔴      |
|          |         |                           |         |                  |            |                   |            | ~~FriendVoiceCall~~    | 🔴      |

## Known Problem
~~- [ ] Signature Service is currently not established, so the login tend to be failed and return code may be 45, you can establish your own sign service by rewriting the `Signature` static class.~~

Thanks KonataDev/TheSnowfield for Provision of Signature API

- Signature of Windows and macOS is missing, you need to figure out by your self
