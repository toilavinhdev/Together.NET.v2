using Together.Application.Features.FeatureTopic.Responses;

namespace Together.Application.Features.FeatureTopic.Queries;

public sealed class GetTopicQuery(Guid id) : IBaseRequest<GetTopicResponse>
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<GetTopicQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<GetTopicQuery, GetTopicResponse>(httpContextAccessor)
    {
        protected override async Task<GetTopicResponse> HandleAsync(GetTopicQuery request, CancellationToken ct)
        {
            var topic = await context.Topics
                .Where(t => t.Id == request.Id)
                .Select(t => t.MapTo<GetTopicResponse>())
                .FirstOrDefaultAsync(ct);

            if (topic is null) throw new DomainException(TogetherErrorCodes.Topic.TopicNotFound);

            return topic;
        }
    }
}