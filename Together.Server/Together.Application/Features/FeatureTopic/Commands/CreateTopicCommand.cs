using Together.Application.Features.FeatureTopic.Responses;
using Together.Domain.Aggregates.TopicAggregate;

namespace Together.Application.Features.FeatureTopic.Commands;

public sealed class CreateTopicCommand : IBaseRequest<CreateTopicResponse>
{
    public Guid ForumId { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public class Validator : AbstractValidator<CreateTopicCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ForumId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<CreateTopicCommand, CreateTopicResponse>(httpContextAccessor)
    {
        protected override async Task<CreateTopicResponse> HandleAsync(CreateTopicCommand request, CancellationToken ct)
        {
            if (!await context.Forums.AnyAsync(x => x.Id == request.ForumId, ct))
                throw new DomainException(TogetherErrorCodes.Forum.ForumNotFound);

            var topic = request.MapTo<Topic>();
            topic.MarkUserCreated(CurrentUserClaims.Id);

            await context.Topics.AddAsync(topic, ct);

            await context.SaveChangesAsync(ct);

            Message = "Created";

            return topic.MapTo<CreateTopicResponse>();
        }
    }
}