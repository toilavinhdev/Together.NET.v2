using System.Linq.Expressions;
using Together.Application.Features.FeatureConversation.Responses;
using Together.Domain.Aggregates.ConversationAggregate;

namespace Together.Application.Features.FeatureConversation.Queries;

public sealed class ConversationQuery : IBaseRequest<ConversationResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public Guid? ConversationId { get; set; }
    
    public class Validator : AbstractValidator<ConversationQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ConversationQuery, ConversationResponse>(httpContextAccessor)
    {
        protected override async Task<ConversationResponse> HandleAsync(ConversationQuery request, CancellationToken ct)
        {
            Expression<Func<Conversation, bool>> whereExpression = c => true;
            whereExpression = whereExpression.And(c => c.ConversationParticipants.Any(cp => cp.UserId == CurrentUserClaims.Id));
            
            var queryable = context.Conversations.AsQueryable();

            if (request.ConversationId is null)
            {
                queryable.Paging(request.PageIndex, request.PageSize);
            }
            else
            {
                whereExpression = whereExpression.And(c => c.Id == request.ConversationId);
            }
            
            queryable = queryable.Where(whereExpression);
            
            var totalRecord = await queryable.LongCountAsync(ct);
            
            var data = await queryable
                .Include(c => c.ConversationParticipants)
                .Include(c => c.Messages)!
                .ThenInclude(m => m.CreatedBy)
                .OrderByDescending(c => c.Messages!.Max(m => m.CreatedAt))
                .Select(c => new ConversationViewModel
                {
                    Id = c.Id,
                    SubId = c.SubId,
                    Type = c.Type,
                    Name = c.Type == ConversationType.Group
                        ? c.Name
                        : c.ConversationParticipants
                            .FirstOrDefault(cp => cp.UserId != CurrentUserClaims.Id)!
                            .User.UserName,
                    Image = c.Type == ConversationType.Group
                        ? null
                        : c.ConversationParticipants
                            .FirstOrDefault(cp => cp.UserId != CurrentUserClaims.Id)!
                            .User.Avatar,
                    LastMessageByUserId = c.Messages!
                        .OrderByDescending(m => m.CreatedAt)
                        .FirstOrDefault()!
                        .CreatedBy.Id,
                    LastMessageByUserName = c.Messages!
                        .OrderByDescending(m => m.CreatedAt)
                        .FirstOrDefault()!
                        .CreatedBy.UserName,
                    LastMessageText = c.Messages!
                        .OrderByDescending(m => m.CreatedAt)
                        .FirstOrDefault()!
                        .Text,
                    LastMessageAt = c.Messages!
                        .OrderByDescending(m => m.CreatedAt)
                        .FirstOrDefault()!
                        .CreatedAt
                })
                .ToListAsync(ct);
            
            return new ConversationResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data
            };
        }
    }
}