using Grpc.Core;
using Kritor.Friend;

namespace Lagrange.Kritor.Services;

public class FriendService : global::Kritor.Friend.FriendService.FriendServiceBase
{
    public override Task<GetFriendListResponse> GetFriendList(GetFriendListRequest request, ServerCallContext context)
    {
        return base.GetFriendList(request, context);
    }

    public override Task<GetFriendProfileCardResponse> GetFriendProfileCard(GetFriendProfileCardRequest request, ServerCallContext context)
    {
        return base.GetFriendProfileCard(request, context);
    }

    public override Task<GetStrangerProfileCardResponse> GetStrangerProfileCard(GetStrangerProfileCardRequest request, ServerCallContext context)
    {
        return base.GetStrangerProfileCard(request, context);
    }

    public override Task<SetProfileCardResponse> SetProfileCard(SetProfileCardRequest request, ServerCallContext context)
    {
        return base.SetProfileCard(request, context);
    }

    public override Task<IsBlackListUserResponse> IsBlackListUser(IsBlackListUserRequest request, ServerCallContext context)
    {
        return base.IsBlackListUser(request, context);
    }

    public override Task<VoteUserResponse> VoteUser(VoteUserRequest request, ServerCallContext context)
    {
        return base.VoteUser(request, context);
    }

    public override Task<GetUidResponse> GetUidByUin(GetUidRequest request, ServerCallContext context)
    {
        return base.GetUidByUin(request, context);
    }

    public override Task<GetUinByUidResponse> GetUinByUid(GetUinByUidRequest request, ServerCallContext context)
    {
        return base.GetUinByUid(request, context);
    }
}