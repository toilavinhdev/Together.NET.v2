namespace Together.Application.Features.FeatureTopic.Commands;

public sealed class UpdateTopicCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public Guid ForumId { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public class Validator : AbstractValidator<UpdateTopicCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ForumId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<UpdateTopicCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdateTopicCommand request, CancellationToken ct)
        {
            var topic = await context.Topics.FirstOrDefaultAsync(t => t.Id == request.Id, ct);

            if (topic is null) throw new DomainException(TogetherErrorCodes.Topic.TopicNotFound);

            topic.ForumId = request.ForumId;
            topic.Name = request.Name;
            topic.Description = request.Description;
            topic.MarkUserModified(CurrentUserClaims.Id);

            context.Topics.Update(topic);
            
            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}