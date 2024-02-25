using System.Text.Json.Serialization;
using Lagrange.Audio;
using Lagrange.Audio.Codec;
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
        if (segment is RecordSegment recordSegment and not { File: "" } && CommonResolver.Resolve(recordSegment.File) is { } record)
        {
            if (ConvertFormat(record, out double time) is { } silk) builder.Record(silk, (int)time);
            else throw new Exception("Encode failed");
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not RecordEntity recordEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(recordEntity.AudioUrl);
    }

    private static byte[]? ConvertFormat(byte[] audio, out double audioTime)
    {
        audioTime = .0d;

        if (AudioHelper.DetectAudio(audio, out var type))
        {
            byte[] audioData;
            switch (type) // Process
            {
                // Amr format
                // We no need to convert it
                case AudioHelper.AudioFormat.Amr:
                {
                    audioData = audio;
                    audioTime = audio.Length / 1607.0;
                    break;
                }

                // Silk v3 for tx use
                case AudioHelper.AudioFormat.TenSilkV3:
                {
                    audioData = audio;
                    audioTime = AudioHelper.GetSilkTime(audio, 1);
                    break;
                }

                // Normal silk v3
                // We need to append a header 0x02
                // and remove 0xFFFF end for it
                case AudioHelper.AudioFormat.SilkV3:
                {
                    audioData = new byte[] { 0x02 }.Concat(audio[..^2]).ToArray();
                    audioTime = AudioHelper.GetSilkTime(audio);
                    break;
                }

                // Cannot convert unknown type
                case AudioHelper.AudioFormat.Unknown:
                {
                    return null;
                }

                // Need to convert
                default:
                case AudioHelper.AudioFormat.Mp3:
                case AudioHelper.AudioFormat.Wav:
                {
                    using var inputStream = new MemoryStream(audio);
                    using var outputStream = new MemoryStream();
                    var audioPipeline = new AudioPipeline();
                    
                    audioPipeline.Add(type switch
                    {
                        AudioHelper.AudioFormat.Mp3 => new Mp3Codec.Decoder(inputStream),  // Decode Mp3 to pcm
                        AudioHelper.AudioFormat.Wav => throw new NotImplementedException(), // Decode Wav to pcm
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