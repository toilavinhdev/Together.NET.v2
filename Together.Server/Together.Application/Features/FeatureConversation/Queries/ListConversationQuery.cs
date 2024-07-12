using Together.Application.Features.FeatureConversation.Responses;

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
            var queryable = context.Conversations.AsQueryable();

            var totalRecord = await queryable.LongCountAsync(ct);

            var data = await queryable
                .Include(c => c.ConversationParticipants)
                .Include(c => c.Messages)!
                .ThenInclude(m => m.CreatedBy)
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

            return new ListConversationResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data
            };
        }
    }
}