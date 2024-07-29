using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureRole.Commands;
using Together.Application.Features.FeatureRole.Queries;
using Together.Application.Features.FeatureRole.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class RoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/role").WithTags("Role");
        
        group.MapGet("/{roleId:guid}", GetRole);
        
        group.MapGet("/list", ListRole);
        
        group.MapPost("/create", CreateRole);
        
        group.MapPost("/assign", AssignRole);
        
        group.MapPut("/update", UpdateRole);
        
        group.MapDelete("/{roleId:guid}", DeleteRole);
    }
    
    [TogetherPermission(TogetherPolicies.Role.View)]
    private static Task<BaseResponse<GetRoleResponse>> GetRole(ISender sender, Guid roleId)
        => sender.Send(new GetRoleQuery(roleId));
    
    [TogetherPermission(TogetherPolicies.Role.View)]
    private static Task<BaseResponse<ListRoleResponse>> ListRole(ISender sender, [AsParameters] ListRoleQuery query)
        => sender.Send(query);

    [TogetherPermission(TogetherPolicies.Role.Create)]
    private static Task<BaseResponse<CreateRoleResponse>> CreateRole(ISender sender, CreateRoleCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Role.Update)]
    private static Task<BaseResponse> UpdateRole(ISender sender, UpdateRoleCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Role.Assign)]
    private static Task<BaseResponse> AssignRole(ISender sender, AssignRoleCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Role.Delete)]
    private static Task<BaseResponse> DeleteRole(ISender sender, Guid roleId)
        => sender.Send(new DeleteRoleCommand(roleId));
}