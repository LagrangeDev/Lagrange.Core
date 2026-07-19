using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_user_profile")]
public class GetUserProfileHandler(BotContext lagrange) : IApiHandler<GetUserProfileHandler.Request, GetUserProfileHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var stranger = await _lagrange.FetchStranger(request.UserId);
        return MilkyApiResponse<Result>.Ok(new Result
        {
            Nickname = stranger.Nickname,
            Qia = stranger.QID,
            Age = (int)stranger.Age,
            Sex = stranger.Gender switch
            {
                BotGender.Male => "male",
                BotGender.Female => "female",
                _ => "unknown",
            },
            Remark = stranger.Remark,
            Bio = stranger.PersonalSign,
            Level = (int)stranger.Level,
            Country = stranger.Country,
            City = stranger.City,
            School = stranger.School ?? string.Empty,
        });
    }

    public class Request
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; }
    }

    public class Result
    {
        [JsonPropertyName("nickname")] public required string Nickname { get; init; }
        [JsonPropertyName("qia")] public required string Qia { get; init; }
        [JsonPropertyName("age")] public required int Age { get; init; }
        [JsonPropertyName("sex")] public required string Sex { get; init; }
        [JsonPropertyName("remark")] public required string Remark { get; init; }
        [JsonPropertyName("bio")] public required string Bio { get; init; }
        [JsonPropertyName("level")] public required int Level { get; init; }
        [JsonPropertyName("country")] public required string Country { get; init; }
        [JsonPropertyName("city")] public required string City { get; init; }
        [JsonPropertyName("school")] public required string School { get; init; }
    }
}