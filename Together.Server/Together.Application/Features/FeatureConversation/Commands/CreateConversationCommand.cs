using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Features.FeatureConversation.Commands;

public sealed class CreateConversationCommand : IBaseRequest<CreateConversationResponse>
{
    public List<Guid> OtherParticipantIds { get; set; } = default!;
    
    public ConversationType Type { get; set; }
    
    public string? Name { get; set; }
    
    public class Validator : AbstractValidator<CreateConversationCommand>
    {
        public Validator()
        {
            RuleFor(x => x.OtherParticipantIds).NotNull().Must(x => x.Count > 0);
            When(x => x.Type == ConversationType.Private, () =>
            {
                RuleFor(x => x.OtherParticipantIds.Count == 1);
            });
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<CreateConversationCommand, CreateConversationResponse>(httpContextAccessor)
    {
        protected override async Task<CreateConversationResponse> HandleAsync(CreateConversationCommand request, CancellationToken ct)
        {
            foreach (var participantId in request.OtherParticipantIds)
            {
                if (!await context.Users.AnyAsync(u => u.Id == participantId, ct))
                    throw new DomainException(TogetherErrorCodes.User.UserNotFound, participantId.ToString());
            }

            if (request.Type == ConversationType.Private)
            {
                var isExist = await context.Conversations
                    .Include(c => c.ConversationParticipants)
                    .AnyAsync(c => 
                        c.ConversationParticipants.Count == 2 &&
                        c.ConversationParticipants.All(cp => 
                                cp.UserId == request.OtherParticipantIds.First() ||  
                                cp.UserId == CurrentUserClaims.Id), 
                        ct);
                if (isExist) throw new DomainException(TogetherErrorCodes.Conversation.PrivateConversationAlreadyExists);
            }

            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ConversationParticipants = request.OtherParticipantIds
                    .Union([CurrentUserClaims.Id])
                    .Select(userId => new ConversationParticipant
                    {
                        UserId = userId
                    })
                    .ToList()
            };
            conversation.MarkUserCreated(CurrentUserClaims.Id);

            await context.Conversations.AddAsync(conversation, ct);

            await context.SaveChangesAsync(ct);

            return conversation.MapTo<CreateConversationResponse>();
        }
    }
}