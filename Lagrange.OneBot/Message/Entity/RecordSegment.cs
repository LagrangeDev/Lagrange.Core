using System.Text.Json.Serialization;
using Konata.Codec.Audio;
using Konata.Codec.Audio.Codecs;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class RecordSegment(string url)
{
    public RecordSegment() : this("") { }

    [JsonPropertyName("file")][CQProperty] public string File { get; set; } = url;

    [JsonPropertyName("url")] public string Url { get; set; } = url;
}

[SegmentSubscriber(typeof(RecordEntity), "record")]
public partial class RecordSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is RecordSegment recordSegment and not { File: "" } && CommonResolver.Resolve(recordSegment.File) is { } record)
        {
            byte[] silk = ConvertFormat(record, out double time);
            builder.Record(silk, (int)Math.Ceiling(time));
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not RecordEntity recordEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(recordEntity.AudioUrl);
    }

    private static byte[] ConvertFormat(byte[] audio, out double audioTime)
    {
        var format = AudioHelper.DetectAudio(audio);
        switch (format) // Process
        {
            // Silk v3 for tx use
            case AudioHelper.AudioFormat.TenSilkV3:
                {
                    audioTime = AudioHelper.GetTenSilkTime(audio);
                    return audio;
                }

            // Amr format
            // We no need to convert it
            case AudioHelper.AudioFormat.Amr:
                {
                    audioTime = audio.Length / 1607.0;
                    return audio;
                }

            // Normal silk v3
            // We need to append a header 0x02
            // and remove 0xFFFF end for it
            case AudioHelper.AudioFormat.SilkV3:
                {
                    audio = [0x02, .. audio.AsSpan(0, audio.Length - 2)];
                    audioTime = AudioHelper.GetTenSilkTime(audio);
                    return audio;
                }

            // Need to convert
            case AudioHelper.AudioFormat.Wav:
            case AudioHelper.AudioFormat.Ogg:
            case AudioHelper.AudioFormat.Mp3:
                {
                    var input = new MemoryStream(audio);
                    var output = new MemoryStream();

                    var pipeline = new AudioPipeline() {
                        format switch {
                            AudioHelper.AudioFormat.Wav => new WavCodec.Decoder(input),
                            AudioHelper.AudioFormat.Ogg => new VorbisCodec.Decoder(input),
                            AudioHelper.AudioFormat.Mp3 => new Mp3Codec.Decoder(input),
                            _ => throw new Exception("Unknown Fromat")
                        },
                        new AudioResampler(AudioInfo.SilkV3()),
                        new SilkV3Codec.Encoder(),
                        output
                    };
                    if (!pipeline.Start().Result) throw new Exception("Encode failed");

                    audioTime = pipeline.GetAudioTime();
                    return output.ToArray();
                }

            // Cannot convert unknown type
            default: throw new Exception("Unknown Fromat");
        }
    }
}