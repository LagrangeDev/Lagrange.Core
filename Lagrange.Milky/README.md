<div align="center">

![Lagrange.Milky](./Resources/banner.svg)

_[Milky](https://github.com/SaltifyDev/milky) protocol implementation based on [Lagrange.Core V2](https://github.com/LagrangeDev/LagrangeV2)_

</div>

## Document

https://lagrangedev.github.io/Lagrange.Milky.Document

## Contribute

### api

Write an implementation of `IApiHandler<TRequest, TResult>`/`INoRequestApiHandler<TResult>`/`INoResultApiHandler<TRequest>` in the folder corresponding to the `Lagrange.Milky\Api\Handlers` category, and add `TRequest` and `TResult` to the collection of `[JsonSerializable]` attributes above `JsonContext` in `Lagrange.Milky\Serialization\Serializer.cs`.

### event

Write an implementation of `IEventConverter<TEvent, TData>` in `Lagrange.Milky\Events\Converters`, and add `TData` to the `[JsonSerializable]` attributes above `JsonContext` in `Lagrange.Milky\Serialization\Serializer.cs`.

## Feature List

- Api
  - [ ] Http
- Event
  - [ ] SSE
  - [x] WebSocket
  - [ ] WebHook

### api

#### system

[x] get_login_info
[x] get_impl_info
[x] get_user_profile
[x] get_friend_list
[x] get_friend_info
[x] get_group_list
[x] get_group_info
[x] get_group_member_list
[x] get_group_member_info
[ ] get_peer_pins
[x] set_peer_pin
[x] set_avatar
[ ] set_nickname
[ ] set_bio
[ ] get_custom_face_url_list
[x] get_cookies
[ ] get_csrf_token

#### message

[x] send_private_message
[x] send_group_message
[x] recall_private_message
[x] recall_group_message
[x] get_message
[x] get_history_messages
  - request
    - [ ] start_message_seq - When message_scene is friend, start_message_seq cannot be null, otherwise the core will be unable to retrieve the latest sequence of friend.
[x] get_resource_temp_url
[ ] get_forwarded_messages
[ ] mark_message_as_read

#### friend

[x] send_friend_nudge
[ ] send_profile_like
[ ] delete_friend
[ ] get_friend_requests
[ ] accept_friend_request
[ ] reject_friend_request

#### group

[x] set_group_name
[ ] set_group_avatar
[x] set_group_member_card
[x] set_group_member_special_title
[ ] set_group_member_admin
[ ] set_group_member_mute
[x] set_group_whole_mute
[x] kick_group_member
[ ] get_group_announcements
[ ] send_group_announcement
[ ] delete_group_announcement
[ ] get_group_essence_messages
[x] set_group_essence_message
[x] quit_group
[x] send_group_message_reaction
  - result
    - reaction_type - core does not implement reaction type
[x] send_group_nudge
[x] get_group_notifications
[x] accept_group_request
[x] reject_group_request
[x] accept_group_invitation
[x] reject_group_invitation

#### file

[x] upload_private_file
  - result
    - [ ] file_id
[x] upload_group_file
  - result
    - [ ] file_id - core did not provide a folder id
[ ] get_private_file_download_url
[x] get_group_file_download_url
[x] get_group_files
[x] move_group_file
[ ] rename_group_file
[x] delete_group_file
[x] create_group_folder
  - result
    - [ ] file_id - core did not provide a folder id
[x] rename_group_folder
[x] delete_group_folder

### event

- [x] bot_offline
- [x] message_receive
- [x] message_recall
- [ ] peer_pin_change
- [x] friend_request
- [x] group_join_request
- [x] group_invited_join_request
- [x] group_invitation
- [ ] friend_nudge
- [ ] friend_file_upload
- [ ] group_admin_change
- [ ] group_essence_message_change
- [x] group_member_increase
- [x] group_member_decrease
- [ ] group_name_change
- [x] group_message_reaction
  - [ ] reaction_type - It will only return "face"
- [ ] group_mute
- [ ] group_whole_mute
- [x] group_nudge
- [ ] group_file_upload

### models

- [x] Friend
- [x] FriendCategory
- [x] Group
- [x] GroupMember
- [ ] GroupAnnouncement
- [x] GroupFile
- [x] GroupFolder
- [ ] FriendRequest
- [x] GroupNotification
- [x] IncomingMessage
  - [x] friend
  - [x] group
- [ ] IncomingForwardedMessage
- [ ] GroupEssenceMessage
- [x] [IncomingSegment](#imcoming-segment)
- [x] OutgoingForwardedMessage
- [x] [OutgoingSegment](#outgoing-segment)

### imcoming segment

- [x] text
- [x] mention
- [x] mention_all
- [ ] face
- [x] reply
- [x] image
- [x] record
- [x] video
- [ ] file
- [x] forward
  - [ ] title
  - [ ] preview
  - [ ] summary
- [ ] market_face
- [x] light_app
- [ ] xml

### outgoing segment

- [x] text
- [x] mention
- [x] mention_all
- [ ] face
- [x] reply
- [x] image
- [x] record
- [x] video
- [x] forward
  - [ ] title
  - [ ] preview
  - [ ] summary
  - [ ] prompt
- [x] light_app