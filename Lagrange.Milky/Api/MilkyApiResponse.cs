using System.Text.Json.Serialization;

namespace Lagrange.Milky.Api;

public class MilkyApiResponse
{
    [JsonPropertyName("status")] public string Status { get; }
    [JsonPropertyName("retcode")] public long Retcode { get; }
    [JsonPropertyName("message")] public string? Message { get; }
    [JsonPropertyName("data")] public object? Data { get; }

    protected MilkyApiResponse(object data)
    {
        Status = "ok";
        Retcode = 0;
        Message = null;
        Data = data;
    }

    public MilkyApiResponse()
    {
        Status = "ok";
        Retcode = 0;
        Message = null;
        Data = new();
    }

    public MilkyApiResponse(long retcode, string message)
    {
        Status = "failed";
        Retcode = retcode;
        Message = message;
        Data = null;
    }
}

public sealed class MilkyApiResponse<TData> : MilkyApiResponse where TData : notnull
{
    public MilkyApiResponse(TData data) : base(data) { }
    public MilkyApiResponse(long retcode, string message) : base(retcode, message) { }
}