using Grpc.Core;
using Kritor.Group;

namespace Lagrange.Kritor.Services;

public class GroupService : global::Kritor.Group.GroupService.GroupServiceBase
{
    public override Task<BanMemberResponse> BanMember(BanMemberRequest request, ServerCallContext context)
    {
        return base.BanMember(request, context);
    }
    
    public override Task<PokeMemberResponse> PokeMember(PokeMemberRequest request, ServerCallContext context)
    {
        return base.PokeMember(request, context);
    }
    
    public override Task<KickMemberResponse> KickMember(KickMemberRequest request, ServerCallContext context)
    {
        return base.KickMember(request, context);
    }
    
    public override Task<LeaveGroupResponse> LeaveGroup(LeaveGroupRequest request, ServerCallContext context)
    {
        return base.LeaveGroup(request, context);
    }
    
    public override Task<ModifyMemberCardResponse> ModifyMemberCard(ModifyMemberCardRequest request, ServerCallContext context)
    {
        return base.ModifyMemberCard(request, context);
    }
    
    public override Task<ModifyGroupNameResponse> ModifyGroupName(ModifyGroupNameRequest request, ServerCallContext context)
    {
        return base.ModifyGroupName(request, context);
    }
    
    public override Task<ModifyGroupRemarkResponse> ModifyGroupRemark(ModifyGroupRemarkRequest request, ServerCallContext context)
    {
        return base.ModifyGroupRemark(request, context);
    }
    
    public override Task<SetGroupAdminResponse> SetGroupAdmin(SetGroupAdminRequest request, ServerCallContext context)
    {
        return base.SetGroupAdmin(request, context);
    }
    
    public override Task<SetGroupUniqueTitleResponse> SetGroupUniqueTitle(SetGroupUniqueTitleRequest request, ServerCallContext context)
    {
        return base.SetGroupUniqueTitle(request, context);
    }
    
    public override Task<SetGroupWholeBanResponse> SetGroupWholeBan(SetGroupWholeBanRequest request, ServerCallContext context)
    {
        return base.SetGroupWholeBan(request, context);
    }
    
    public override Task<GetGroupInfoResponse> GetGroupInfo(GetGroupInfoRequest request, ServerCallContext context)
    {
        return base.GetGroupInfo(request, context);
    }
    
    public override Task<GetGroupListResponse> GetGroupList(GetGroupListRequest request, ServerCallContext context)
    {
        return base.GetGroupList(request, context);
    }
    
    public override Task<GetGroupMemberInfoResponse> GetGroupMemberInfo(GetGroupMemberInfoRequest request, ServerCallContext context)
    {
        return base.GetGroupMemberInfo(request, context);
    }
    
    public override Task<GetGroupMemberListResponse> GetGroupMemberList(GetGroupMemberListRequest request, ServerCallContext context)
    {
        return base.GetGroupMemberList(request, context);
    }
    
    public override Task<GetProhibitedUserListResponse> GetProhibitedUserList(GetProhibitedUserListRequest request, ServerCallContext context)
    {
        return base.GetProhibitedUserList(request, context);
    }
    
    public override Task<GetRemainCountAtAllResponse> GetRemainCountAtAll(GetRemainCountAtAllRequest request, ServerCallContext context)
    {
        return base.GetRemainCountAtAll(request, context);
    }
    
    public override Task<GetNotJoinedGroupInfoResponse> GetNotJoinedGroupInfo(GetNotJoinedGroupInfoRequest request, ServerCallContext context)
    {
        return base.GetNotJoinedGroupInfo(request, context);
    }
    
    public override Task<GetGroupHonorResponse> GetGroupHonor(GetGroupHonorRequest request, ServerCallContext context)
    {
        return base.GetGroupHonor(request, context);
    }
}