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
        
        group.MapGet("/list", ListPrefix);
        
        group.MapPost("/create", CreatePrefix);
    }
    
    [TogetherPermission(TogetherPolicies.Prefix.View)]
    private static Task<BaseResponse<List<PrefixViewModel>>> ListPrefix(ISender sender)
        => sender.Send(new ListPrefixQuery());
    
    [TogetherPermission(TogetherPolicies.Prefix.Create)]
    private static Task<BaseResponse<CreatePrefixResponse>> CreatePrefix(ISender sender, CreatePrefixCommand command)
        => sender.Send(command);
}