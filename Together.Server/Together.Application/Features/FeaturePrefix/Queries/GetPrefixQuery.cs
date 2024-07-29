using Together.Application.Features.FeaturePrefix.Responses;

namespace Together.Application.Features.FeaturePrefix.Queries;

public sealed class GetPrefixQuery(Guid id) : IBaseRequest<PrefixViewModel>
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<GetPrefixQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<GetPrefixQuery, PrefixViewModel>(httpContextAccessor)
    {
        protected override async Task<PrefixViewModel> HandleAsync(GetPrefixQuery request, CancellationToken ct)
        {
            var prefix = await context.Prefixes
                .Where(p => p.Id == request.Id)
                .Select(p => p.MapTo<PrefixViewModel>())
                .FirstOrDefaultAsync(ct);

            if (prefix is null) throw new DomainException(TogetherErrorCodes.Prefix.PrefixNotFound);

            return prefix;
        }
    }
}