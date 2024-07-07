using System.Linq.Expressions;
using Together.Application.Features.FeaturePost.Responses;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Application.Features.FeaturePost.Queries;

public sealed class ListPostQuery : IBaseRequest<ListPostResponse>, IPaginationRequest
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public Guid? TopicId { get; set; }
    
    public Guid? UserId { get; set; }
    
    public class Validator : AbstractValidator<ListPostQuery>
    {
        public Validator()
        {
            Include(new PaginationValidator());
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<ListPostQuery, ListPostResponse>(httpContextAccessor)
    {
        protected override async Task<ListPostResponse> HandleAsync(ListPostQuery request, CancellationToken ct)
        {
            var extra = new Dictionary<string, string>();
            
            Expression<Func<Post, bool>> whereExpression = x => true;
            
            if (request.TopicId is not null)
            {
                var topic = await context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId, ct);
                if (topic is null) throw new DomainException(TogetherErrorCodes.Topic.TopicNotFound);
                whereExpression = whereExpression.And(x => x.TopicId == request.TopicId);
                extra.Add("topicId", topic.Id.ToString());
                extra.Add("topicName", topic.Name);
            }

            if (request.UserId is not null)
            {
                if (!await context.Users.AnyAsync(f => f.Id == request.UserId, ct))
                    throw new DomainException(TogetherErrorCodes.User.UserNotFound);
                whereExpression = whereExpression.And(x => x.CreatedById == request.UserId);
            }
            
            var query = context.Posts
                .Where(whereExpression);

            var totalRecord = await query.LongCountAsync(ct);
            
            var data = await query
                .Include(p => p.CreatedBy)
                .Include(p => p.Topic)
                .Include(p => p.Prefix)
                .Include(p => p.Replies)
                .OrderBy(p => p.CreatedAt)
                .Paging(request.PageIndex, request.PageSize)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    SubId = p.SubId,
                    ForumId = p.ForumId,
                    TopicId = p.Topic.Id,
                    TopicName = p.Topic.Name,
                    PrefixId = p.Prefix!.Id,
                    PrefixName = p.Prefix!.Name,
                    PrefixBackground = p.Prefix!.Background,
                    PrefixForeground = p.Prefix!.Foreground,
                    Title = p.Title,
                    Body = p.Body,
                    CreatedById = p.CreatedBy.Id,
                    CreatedByUserName = p.CreatedBy.UserName,
                    CreatedByAvatar = p.CreatedBy.Avatar,
                    CreatedAt = p.CreatedAt,
                    ReplyCount = p.Replies!.LongCount()
                })
                .ToListAsync(ct);

            return new ListPostResponse
            {
                Pagination = new Pagination(request.PageIndex, request.PageSize, totalRecord),
                Result = data,
                Extra = extra
            };
        }
    }
}