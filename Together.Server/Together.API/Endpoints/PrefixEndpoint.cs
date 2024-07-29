using MediatR;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeaturePrefix.Commands;
using Together.Application.Features.FeaturePrefix.Queries;
using Together.Application.Features.FeaturePrefix.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class PrefixEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/prefix").WithTags("Prefix");
        
        group.MapGet("/{prefixId:guid}", GetPrefix);
        
        group.MapGet("/list", ListPrefix);

        group.MapPost("/create", CreatePrefix);
            
        group.MapPut("/update", UpdatePrefix);
        
        group.MapDelete("/{prefixId:guid}", DeletePrefix);
    }
    
    [TogetherPermission(TogetherPolicies.Prefix.View)]
    private static Task<BaseResponse<PrefixViewModel>> GetPrefix(ISender sender, Guid prefixId)
        => sender.Send(new GetPrefixQuery(prefixId));
    
    [TogetherPermission(TogetherPolicies.Prefix.View)]
    private static Task<BaseResponse<List<PrefixViewModel>>> ListPrefix(ISender sender)
        => sender.Send(new ListPrefixQuery());
    
    [TogetherPermission(TogetherPolicies.Prefix.Create)]
    private static Task<BaseResponse<CreatePrefixResponse>> CreatePrefix(ISender sender, CreatePrefixCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Prefix.Update)]
    private static Task<BaseResponse> UpdatePrefix(ISender sender, UpdatePrefixCommand command)
        => sender.Send(command);
    
    [TogetherPermission(TogetherPolicies.Prefix.Delete)]
    private static Task<BaseResponse> DeletePrefix(ISender sender, Guid prefixId)
        => sender.Send(new DeletePrefixCommand(prefixId));
}