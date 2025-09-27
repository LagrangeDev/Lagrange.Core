<div align="center">

![Lagrange.Core](https://socialify.git.ci/KonataDev/Lagrange.Core/image?description=1&descriptionEditable=An%20Implementation%20of%20NTQQ%20Protocol%2C%20with%20Pure%20C%23%2CDerived%20from%20Konata.Core&font=Jost&forks=1&issues=1&logo=https%3A%2F%2Fstatic.live.moe%2Flagrange.jpg&name=1&pattern=Diagonal%20Stripes&pulls=1&stargazers=1&theme=Auto)
[![Core](https://img.shields.io/badge/Lagrange-Core-blue)](https://github.com/LagrangeDev/Lagrange.Core/pkgs/nuget/Lagrange.Core)
[![OneBot](https://img.shields.io/badge/Lagrange-OneBot-blue)](https://lagrangedev.github.io/Lagrange.Doc/Lagrange.OneBot/)
![C#](https://img.shields.io/badge/Core-%20.NET_6-blue)
![C#](https://img.shields.io/badge/OneBot-%20.NET_7-blue)

[![License](https://img.shields.io/static/v1?label=LICENSE&message=GPL-3.0&color=lightrey)](/LICENSE)
[![Telegram](https://img.shields.io/badge/Chat-Telegram-27A7E7)](https://t.me/+6HNTeJO0JqtlNmRl)

[![Image](https://trendshift.io/api/badge/repositories/3486)](https://trendshift.io/repositories/3486)

**&gt; English &lt;** | [简体中文](README_zh.md)

</div>

## Related Projects

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

# Lagrange.Core

## Document V1

[Lagrange.Core Document](https://lagrangedev.github.io/Lagrange.Doc/v1/Lagrange.Core/)

## Features List

<Details>

| Protocol | Support | Login                     | Support | Messages  | Support | Operations        | Support | Events              | Support |
| -------- | :-----: | ------------------------- | :-----: | :-------- | :-----: | :---------------- | :-----: | :------------------ | :-----: |
| Windows  |   🟢    | QrCode                    |   🟢    | Images    |   🟢    | Poke              |   🟢    | Captcha             |   🟢    |
| macOS    |   🟢    | Password                  |   🔴    | Text / At |   🟢    | Recall            |   🟢    | BotOnline           |   🟢    |
| Linux    |   🟢    | EasyLogin                 |   🟢    | Records   |   🟢    | Leave Group       |   🟢    | BotOffline          |   🟢    |
|          |         | UnusalDevice<br/>Password |   🔴    | QFace     |   🟢    | Set Special Title |   🟢    | Message             |   🟢    |
|          |         | UnusalDevice<br/>Easy     |   🟢    | Json      |   🟢    | Kick Member       |   🟢    | Poke                |   🟢    |
|          |         | NewDeviceVerify           |   🔴    | Xml       |   🟢    | Mute Member       |   🟢    | MessageRecall       |   🟢    |
|          |         |                           |         | Forward   |   🟢    | Set Admin         |   🟢    | GroupMemberDecrease |   🟢    |
|          |         |                           |         | Video     |   🟢    | Friend Request    |   🟢    | GroupMemberIncrease |   🟢    |
|          |         |                           |         | Reply     |   🟢    | Group Request     |   🟢    | GroupPromoteAdmin   |   🟢    |
|          |         |                           |         | File      |   🟢    | ~~Voice Call~~    |   🔴    | GroupInvite         |   🟢    |
|          |         |                           |         | Poke      |   🟢    | Client Key        |   🟢    | GroupRequestJoin    |   🟢    |
|          |         |                           |         | LightApp  |   🟢    | Cookies           |   🟢    | FriendRequest       |   🟢    |
|          |         |                           |         |           |         | Send Message      |   🟢    | ~~FriendTyping~~    |   🔴    |
|          |         |                           |         |           |         |                   |         | ~~FriendVoiceCall~~ |   🔴    |

</Details>

# Lagrange.OneBot

## Document

[Lagrange.OneBot Document](https://lagrangedev.github.io/Lagrange.Doc/v1/Lagrange.OneBot/)

[Docker guide](Docker.md)

## SignServer

https://sign.lagrangecore.org/api/sign

Thanks for 外国热心网友 for Provision of Azure Servlet

** Built-in SignServer is now provided, Enjoy! **