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

    [JsonPropertyName("file")] [CQProperty] public string File { get; set; } = url;

    [JsonPropertyName("url")] public string Url { get; set; }  = url;
}

[SegmentSubscriber(typeof(RecordEntity), "record")]
public partial class RecordSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is RecordSegment recordSegment and not { File: "" } && CommonResolver.ResolveStream(recordSegment.File) is { } record)
        {
            if (ConvertFormat(record, out double time) is { } silk)
            {
                builder.Record(silk, (int)time);
            }
            else throw new Exception("Encode failed");
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not RecordEntity recordEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(recordEntity.AudioUrl);
    }

    private static byte[]? ConvertFormat(Stream audio, out double audioTime)
    {
        audioTime = .0d;
        var head = new byte[64];
        _ = audio.Read(head.AsSpan());
        audio.Seek(0, SeekOrigin.Begin);

        if (AudioHelper.DetectAudio(head, out var type))
        {
            byte[] audioData;
            switch (type) // Process
            {
                // Amr format
                // We no need to convert it
                case AudioHelper.AudioFormat.Amr:
                {
                    var ms = new MemoryStream();
                    audio.CopyTo(ms);
                    audioData = ms.ToArray(); 
                    audioTime = audio.Length / 1607.0;
                    break;
                }

                // Silk v3 for tx use
                case AudioHelper.AudioFormat.TenSilkV3:
                {
                    var ms = new MemoryStream();
                    audio.CopyTo(ms);
                    audioData = ms.ToArray();
                    audioTime = AudioHelper.GetSilkTime(head, 1);
                    break;
                }

                // Normal silk v3
                // We need to append a header 0x02
                // and remove 0xFFFF end for it
                case AudioHelper.AudioFormat.SilkV3:
                {
                    var raw = new byte[audio.Length - 2];
                    _ = audio.Read(raw.AsSpan());
                    
                    audioData = new byte[] { 0x02 }.Concat(raw[..^2]).ToArray();
                    audioTime = AudioHelper.GetSilkTime(head);
                    break;
                }

                // Cannot convert unknown type
                case AudioHelper.AudioFormat.Unknown:
                {
                    return null;
                }

                // Need to convert
                default:
                case AudioHelper.AudioFormat.Ogg:
                case AudioHelper.AudioFormat.Mp3:
                case AudioHelper.AudioFormat.Wav:
                {
                    using var outputStream = new MemoryStream();
                    var audioPipeline = new AudioPipeline();
                    
                    audioPipeline.Add(type switch
                    {
                        AudioHelper.AudioFormat.Mp3 => new Mp3Codec.Decoder(audio),  // Decode Mp3 to pcm
                        AudioHelper.AudioFormat.Wav => new WavCodec.Decoder(audio), // Decode Wav to pcm
                        AudioHelper.AudioFormat.Ogg => new VorbisCodec.Decoder(audio),
                        _ => throw new NotImplementedException()
                    }); // Resample audio to silkv3
                    audioPipeline.Add(new AudioResampler(AudioInfo.SilkV3())); // Encode pcm to silkv3
                    audioPipeline.Add(new SilkV3Codec.Encoder()); // Output stream
                    audioPipeline.Add(outputStream);

                    // Start pipeline
                    if (!audioPipeline.Start().Result) return null;
                    
                    audioData = outputStream.ToArray(); // Set audio information
                    audioTime = audioPipeline.GetAudioTime();
                    audioPipeline.Dispose();

                    break;
                }
            }

            return audioData;
        }

        return null;
    }
}