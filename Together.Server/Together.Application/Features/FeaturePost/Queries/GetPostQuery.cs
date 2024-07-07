using Together.Application.Features.FeaturePost.Responses;

namespace Together.Application.Features.FeaturePost.Queries;

public sealed class GetPostQuery(Guid id) : IBaseRequest<GetPostResponse>
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<GetPostQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, IRedisService redisService) 
        : BaseRequestHandler<GetPostQuery, GetPostResponse>(httpContextAccessor)
    {
        protected override async Task<GetPostResponse> HandleAsync(GetPostQuery request, CancellationToken ct)
        {
            var post = await context.Posts
                .Include(p => p.Topic)
                .Include(p => p.CreatedBy)
                .Include(p => p.Prefix)
                .Include(p => p.PostVotes)
                .Select(post => new GetPostResponse
                {
                    Id = post.Id,
                    SubId = post.SubId,
                    TopicId = post.Topic.Id,
                    TopicName = post.Topic.Name,
                    PrefixName = post.Prefix!.Name,
                    PrefixForeground = post.Prefix.Foreground,
                    PrefixBackground = post.Prefix.Background,
                    Title = post.Title,
                    Body = post.Body,
                    CreatedAt = post.CreatedAt,
                    CreatedById = post.CreatedBy.Id,
                    CreatedByUserName = post.CreatedBy.UserName,
                    CreatedByAvatar = post.CreatedBy.Avatar,
                    ReplyCount = post.Replies!.LongCount(),
                    VoteUpCount = post.PostVotes!.LongCount(vote => vote.Type == VoteType.UpVote),
                    VoteDownCount = post.PostVotes!.LongCount(vote => vote.Type == VoteType.DownVote),
                    ViewCount = 0,
                    Voted = post.PostVotes!.FirstOrDefault(vote => vote.CreatedById == CurrentUserClaims.Id)!.Type
                })
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (post is null) throw new DomainException(TogetherErrorCodes.Post.PostNotFound);
            
            var viewCount = await redisService.GetAsync(TogetherRedisKeys.GetPostViewKey(post.SubId));
            post.ViewCount = viewCount!.ToLong();
            
            // TODO: Increment post view count
            var key = TogetherRedisKeys.GetUserViewPostKey(post.SubId, CurrentUserClaims.SubId);

            if (!await redisService.ExistsAsync(key))
            {
                await redisService.IncrementAsync(TogetherRedisKeys.GetPostViewKey(post.SubId));
                await redisService.SetAsync(key, 0, TimeSpan.FromMinutes(TogetherBusinessConfigs.UserViewPostDurationInMinutes));
            }

            return post;
        }
    }
}