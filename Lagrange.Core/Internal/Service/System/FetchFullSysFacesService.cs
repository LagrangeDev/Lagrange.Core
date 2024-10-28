using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

// TODO: onebot11 API (?
[EventSubscribe(typeof(FetchFullSysFacesEvent))]
[Service("OidbSvcTrpcTcp.0x9154_1")]
internal class FetchFullSysFacesService : BaseService<FetchFullSysFacesEvent>
{
    protected override bool Build(FetchFullSysFacesEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x9154_1>(new OidbSvcTrpcTcp0x9154_1
        {
            Field1 = 0,
            Field2 = 7,
            Field3 = "0",
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchFullSysFacesEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x9154_1Response>>(input);
        var response = packet.Body;
        
        var emojiPackList = new[] { response.CommonFace, response.SpecialBigFace }
            .SelectMany(content => content.EmojiList)
            .Select(
                emojiList => new SysFacePackEntry(
                    emojiList.EmojiPackName, emojiList.EmojiDetail.Select(
                        emoji => new SysFaceEntry(
                            emoji.QSid, emoji.QDes, emoji.EMCode, emoji.QCid, emoji.AniStickerType,
                            emoji.AniStickerPackId, emoji.AniStickerId, emoji.Url?.BaseUrl,
                            emoji.EmojiNameAlias, emoji.AniStickerWidth, emoji.AniStickerHeight
                        )
                    ).ToArray()
                )
            ).ToList();
        
        var magicFaceList = response.SpecialMagicFace.Field1.EmojiList.Select(
            emoji => new SysFaceEntry(
                emoji.QSid, emoji.QDes, emoji.EMCode, emoji.QCid, emoji.AniStickerType,
                emoji.AniStickerPackId, emoji.AniStickerId, emoji.Url?.BaseUrl,
                emoji.EmojiNameAlias, emoji.AniStickerWidth, emoji.AniStickerHeight
            )
        ).ToList();
        
        var magicFacePackEntry = new SysFacePackEntry("MagicFace", magicFaceList.ToArray());
        emojiPackList.Add(magicFacePackEntry);

        output = FetchFullSysFacesEvent.Result((int)packet.ErrorCode, emojiPackList);
        extraEvents = null;
        return true;
    }
}