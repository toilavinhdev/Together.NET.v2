using MediatR;
using Microsoft.AspNetCore.Authorization;
using Together.API.Extensions;
using Together.Application.Authorization;
using Together.Application.Features.FeatureReport.Queries;
using Together.Application.Features.FeatureReport.Responses;
using Together.Shared.Constants;
using Together.Shared.ValueObjects;

namespace Together.API.Endpoints;

public sealed class ReportEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/report").WithTags("Report");

        group.MapPost("/statistics", Statistics);
        
        group.MapGet("/prefix", PrefixReport);
        
        group.MapGet("/daily-user", DailyUserReport);
    }

    [Authorize]
    private static Task<BaseResponse<Dictionary<string, object>>> Statistics(ISender sender, StatisticsQuery query)
        => sender.Send(query);
    
    [TogetherPermission(TogetherPolicies.Management.ViewDashboard)]
    private static Task<BaseResponse<List<PrefixReportResponse>>> PrefixReport(ISender sender)
        => sender.Send(new PrefixReportQuery());
    
    [TogetherPermission(TogetherPolicies.Management.ViewDashboard)]
    private static Task<BaseResponse<List<DailyUserReportResponse>>> DailyUserReport(ISender sender, [AsParameters] DailyUserReportQuery query)
        => sender.Send(query);
}