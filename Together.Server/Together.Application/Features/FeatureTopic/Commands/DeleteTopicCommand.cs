namespace Together.Application.Features.FeatureTopic.Commands;

public sealed class DeleteTopicCommand(Guid id) : IBaseRequest
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<DeleteTopicCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<DeleteTopicCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeleteTopicCommand request, CancellationToken ct)
        {
            var topic = await context.Topics.FirstOrDefaultAsync(t => t.Id == request.Id, ct);

            if (topic is null) throw new DomainException(TogetherErrorCodes.Topic.TopicNotFound);

            context.Topics.Remove(topic);

            await context.SaveChangesAsync(ct);

            Message = "Deleted";
        }
    }
}