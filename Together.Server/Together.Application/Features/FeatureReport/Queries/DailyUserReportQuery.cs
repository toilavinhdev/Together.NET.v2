using Together.Application.Features.FeatureReport.Responses;

namespace Together.Application.Features.FeatureReport.Queries;

public sealed class DailyUserReportQuery : IBaseRequest<List<DailyUserReportResponse>>
{
    public DateTimeOffset From { get; set; }
    
    public DateTimeOffset To { get; set; }
    
    public class Validator : AbstractValidator<DailyUserReportQuery>
    {
        public Validator()
        {
            RuleFor(x => x.From).NotEmpty();
            RuleFor(x => x.To)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.From);
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<DailyUserReportQuery, List<DailyUserReportResponse>>(httpContextAccessor)
    {
        protected override async Task<List<DailyUserReportResponse>> HandleAsync(DailyUserReportQuery request, CancellationToken ct)
        {
            var report = new List<DailyUserReportResponse>();

            var days = DateTimeExtensions.GetDaysInRange(request.From, request.To);

            foreach (var day in days)
            {
                var totalNewUser = await context.Users
                    .LongCountAsync(u => u.CreatedAt >= day.StartOfDayUtc() && u.CreatedAt <= day.EndOfDayUtc(), ct);
                
                var totalUser = await context.Users
                    .LongCountAsync(u => u.CreatedAt <= day.EndOfDayUtc(), ct);
                
                report.Add(new DailyUserReportResponse
                {
                    Day = day,
                    TotalNewUser = totalNewUser,
                    TotalUser = totalUser
                });
            }
            
            return report;
        }
    }
}