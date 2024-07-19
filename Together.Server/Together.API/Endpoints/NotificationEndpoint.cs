using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Features.FeatureNotification.Queries;
using Together.Application.Features.FeatureNotification.Responses;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class NotificationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/notification").WithTags("Notification");

        group.MapGet("/list", ListNotification);
    }

    [Authorize]
    private static Task<BaseResponse<ListNotificationResponse>> ListNotification(ISender sender, [AsParameters] ListNotificationQuery query)
        => sender.Send(query);
}