namespace Together.Application.Features.FeaturePrefix.Commands;

public sealed class UpdatePrefixCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;

    public string Foreground { get; set; } = default!;

    public string Background { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdatePrefixCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Foreground).NotEmpty();
            RuleFor(x => x.Background).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<UpdatePrefixCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdatePrefixCommand request, CancellationToken ct)
        {
            var prefix = await context.Prefixes.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (prefix is null) throw new DomainException(TogetherErrorCodes.Prefix.PrefixNotFound);

            prefix.Name = request.Name;
            prefix.Background = request.Background;
            prefix.Foreground = request.Foreground;
            prefix.MarkUserModified(CurrentUserClaims.Id);

            context.Prefixes.Update(prefix);

            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}