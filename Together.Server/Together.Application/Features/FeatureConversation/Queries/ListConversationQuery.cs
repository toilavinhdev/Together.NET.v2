using System.Linq.Expressions;
using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Features.FeatureConversation.Queries;

public sealed class ListConversationQuery : IBaseRequest<ListConversationResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public class Validator : AbstractValidator<ListConversationQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListConversationQuery, ListConversationResponse>(httpContextAccessor)
    {
        protected override async Task<ListConversationResponse> HandleAsync(ListConversationQuery request, CancellationToken ct)
        {
            Expression<Func<Conversation, bool>> whereExpression = c => true;
            whereExpression = whereExpression.And(c => c.ConversationParticipants.Any(cp => cp.UserId == CurrentUserClaims.Id));
            
            var queryable = context.Conversations.AsQueryable();
            
            var totalRecord = await queryable.LongCountAsync(ct);
            
            var data = await queryable
                .Include(c => c.ConversationParticipants)
                .Include(c => c.Messages)!
                .ThenInclude(m => m.CreatedBy)
                .Where(whereExpression)
                .OrderByDescending(c => c.Messages!.Max(m => m.CreatedAt))
                .Paging(request.PageIndex, request.PageSize)
                .Select(ConversationExpressions.ConversationViewModelSelector(CurrentUserClaims))
                .ToListAsync(ct);
            
            return new ListConversationResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data
            };
        }
    }
}