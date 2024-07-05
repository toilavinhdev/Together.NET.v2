using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Features.FeatureReport.Queries;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class ReportEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/report").WithTags("Report");

        group.MapGet("/statistic", Statistic);
    }

    [Authorize]
    private static Task<BaseResponse<object>> Statistic(ISender sender)
        => sender.Send(new StatisticQuery());
}