using System.Linq.Expressions;
using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Features.FeatureConversation.Queries;

public sealed class GetConversationQuery : IBaseRequest<ConversationViewModel?>
{
    public Guid? ConversationId { get; set; }
    
    public Guid? PrivateReceiverId { get; set; }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<GetConversationQuery, ConversationViewModel?>(httpContextAccessor)
    {
        protected override async Task<ConversationViewModel?> HandleAsync(GetConversationQuery request, CancellationToken ct)
        {
            Expression<Func<Conversation, bool>> whereExpression = c => true;

            if (request.ConversationId is not null)
            {
                whereExpression = whereExpression.And(c => c.Id == request.ConversationId);
            }

            if (request.PrivateReceiverId is not null)
            {
                whereExpression = whereExpression.And(c => 
                    c.Type == ConversationType.Private &&
                    c.ConversationParticipants.Count == 2 &&
                    c.ConversationParticipants.All(cp => 
                        cp.UserId == request.PrivateReceiverId || 
                        cp.UserId == CurrentUserClaims.Id));
            }
            
            var conversation = await context.Conversations
                .Include(c => c.ConversationParticipants)
                .Where(whereExpression)
                .Select(ConversationExpressions.ConversationViewModelSelector(CurrentUserClaims))
                .FirstOrDefaultAsync(ct);

            return conversation;
        }
    }
}