namespace Together.Application.Features.FeaturePrefix.Commands;

public sealed class DeletePrefixCommand(Guid id) : IBaseRequest
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<DeletePrefixCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, IRedisService redisService) 
        : BaseRequestHandler<DeletePrefixCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeletePrefixCommand request, CancellationToken ct)
        {
            var prefix = await context.Prefixes.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (prefix is null) throw new DomainException(TogetherErrorCodes.Prefix.PrefixNotFound);

            context.Prefixes.Remove(prefix);

            await context.SaveChangesAsync(ct);

            await redisService.KeyDeleteAsync(TogetherRedisKeys.PrefixKey(prefix.Id));

            Message = "Deleted";
        }
    }
}