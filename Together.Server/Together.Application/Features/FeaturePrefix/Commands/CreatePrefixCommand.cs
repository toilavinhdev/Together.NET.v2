using Together.Application.Features.FeaturePrefix.Responses;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Application.Features.FeaturePrefix.Commands;

public sealed class CreatePrefixCommand : IBaseRequest<CreatePrefixResponse>
{
    public string Name { get; set; } = default!;

    public string Foreground { get; set; } = default!;

    public string Background { get; set; } = default!;
    
    public class Validator : AbstractValidator<CreatePrefixCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Foreground).NotEmpty();
            RuleFor(x => x.Background).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<CreatePrefixCommand, CreatePrefixResponse>(httpContextAccessor)
    {
        protected override async Task<CreatePrefixResponse> HandleAsync(CreatePrefixCommand request, CancellationToken ct)
        {
            var prefix = request.MapTo<Prefix>();
            prefix.MarkUserCreated(CurrentUserClaims.Id);

            await context.Prefixes.AddAsync(prefix, ct);

            await context.SaveChangesAsync(ct);

            Message = "Created";
            
            return prefix.MapTo<CreatePrefixResponse>();
        }
    }
}