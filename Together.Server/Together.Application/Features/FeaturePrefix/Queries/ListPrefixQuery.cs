using Together.Application.Features.FeaturePrefix.Responses;

namespace Together.Application.Features.FeaturePrefix.Queries;

public sealed class ListPrefixQuery : IBaseRequest<List<PrefixViewModel>>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListPrefixQuery, List<PrefixViewModel>>(httpContextAccessor)

    {
        protected override async Task<List<PrefixViewModel>> HandleAsync(ListPrefixQuery request, CancellationToken ct)
        {
            return await context.Prefixes
                .Select(prefix => prefix.MapTo<PrefixViewModel>())
                .ToListAsync(ct);
        }
    }
}