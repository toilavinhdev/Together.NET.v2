using Together.Application.Features.FeaturePrefix.Responses;

namespace Together.Application.Features.FeaturePrefix.Queries;

public sealed class ListPrefixQuery : IBaseRequest<List<PrefixViewModel>>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, IRedisService redisService) 
        : BaseRequestHandler<ListPrefixQuery, List<PrefixViewModel>>(httpContextAccessor)
    {
        private const bool ReadFromCache = true; // Check performance
        
        protected override async Task<List<PrefixViewModel>> HandleAsync(ListPrefixQuery request, CancellationToken ct)
        {
            if (ReadFromCache)
            {
                var cachedPrefixKeys = await redisService.KeysByPatternAsync(
                    TogetherRedisKeys.PrefixKey("*"));

                if (cachedPrefixKeys.Count != 0)
                {
                    var cachedPrefixes = new List<PrefixViewModel>();
                
                    foreach (var cachedPrefixId in cachedPrefixKeys.Select(key => key.Split(":")[1]))
                    {
                        var cachedPrefix = await redisService.StringGetAsync<PrefixViewModel>(TogetherRedisKeys.PrefixKey(cachedPrefixId));
                        if (cachedPrefix is null) continue;
                        cachedPrefixes.Add(cachedPrefix!);
                    }

                    return cachedPrefixes;
                }
            }
            
            var prefixes = await context.Prefixes
                .Select(prefix => prefix.MapTo<PrefixViewModel>())
                .ToListAsync(ct);

            foreach (var prefix in prefixes)
            {
                await redisService.StringSetAsync(TogetherRedisKeys.PrefixKey(prefix.Id), prefix);
            }
            
            return prefixes;
        }
    }
}