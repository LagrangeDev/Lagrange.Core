using System.Text.Json.Serialization;

namespace Lagrange.Milky.Api;

public class MilkyApiResponse
{
    [JsonPropertyName("status")] public required string Status { get; init; }
    [JsonPropertyName("retcode")] public required long Retcode { get; init; }
    [JsonPropertyName("message")] public required string? Message { get; init; }
    [JsonPropertyName("data")] public required object? Data { get; init; }
}

public class MilkyApiResponse<TData> : MilkyApiResponse where TData : notnull
{
    public static MilkyApiResponse<TData> Ok(TData data) => new()
    {
        Status = "ok",
        Retcode = 0,
        Message = null,
        Data = data,
    };

    public static MilkyApiResponse<TData> Failed(long retcode, string message) => new()
    {
        Status = "failed",
        Retcode = retcode,
        Message = message,
        Data = null,
    };
}