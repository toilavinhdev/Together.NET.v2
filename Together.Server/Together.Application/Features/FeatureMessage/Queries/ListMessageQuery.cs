using System.Linq.Expressions;
using Together.Application.Features.FeatureMessage.Responses;
using Together.Domain.Aggregates.MessageAggregate;

namespace Together.Application.Features.FeatureMessage.Queries;

public sealed class ListMessageQuery : IBaseRequest<ListMessageResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public Guid ConversationId { get; set; }
    
    public class Validator : AbstractValidator<ListMessageQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
            RuleFor(x => x.ConversationId).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, IRedisService redisService)
        : BaseRequestHandler<ListMessageQuery, ListMessageResponse>(httpContextAccessor)
    {
        protected override async Task<ListMessageResponse> HandleAsync(ListMessageQuery request, CancellationToken ct)
        {
            var conversation= await context.Conversations.FirstOrDefaultAsync(c => c.Id == request.ConversationId, ct);
            if (conversation is null) throw new DomainException(TogetherErrorCodes.Conversation.ConversationNotFound);
            
            var extra = new Dictionary<string, object>();
            extra.Add("conversationType", conversation.Type);

            Expression<Func<Message, bool>> whereExpression = m => true;
            whereExpression = whereExpression.And(m => m.DeletedAt == null);
            whereExpression = whereExpression.And(m => m.ConversationId == request.ConversationId);

            var queryable = context.Messages
                .Where(whereExpression)
                .AsQueryable();

            var totalRecord = await queryable.LongCountAsync(ct);

            var data = await queryable
                .OrderByDescending(m => m.CreatedAt)
                .Paging(request.PageIndex, request.PageSize)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SubId = m.SubId,
                    ConversationId = m.ConversationId,
                    Text = m.Text,
                    CreatedAt = m.CreatedAt,
                    CreatedById = m.CreatedBy.Id,
                    CreatedByUserName = m.CreatedBy.UserName,
                    CreatedByAvatar = m.CreatedBy.Avatar
                })
                .ToListAsync(ct);

            switch (conversation.Type)
            {
                case ConversationType.Group:
                    extra.Add("conversationName", conversation.Name!);
                    break;
                case ConversationType.Private:
                    var receiver = await context.ConversationParticipants
                        .Include(conversationParticipant => conversationParticipant.User)
                        .FirstOrDefaultAsync(cp => cp.ConversationId == request.ConversationId && cp.UserId != CurrentUserClaims.Id, ct);
                    if (receiver is null) throw new NullReferenceException();
                    extra.Add("conversationName", receiver.User.UserName);
                    extra.Add("conversationImage", receiver.User.Avatar!);
                    extra.Add("userId", receiver.User.Id);
                    var userOnline = await redisService.SetContainsAsync(TogetherRedisKeys.OnlineUserKey(), receiver.User.Id.ToString());
                    extra.Add("userOnline", userOnline);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new ListMessageResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data,
                Extra = extra
            };
        }
    }
}