using Together.Application.Features.FeaturePost.Responses;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Application.Features.FeaturePost.Commands;

public sealed class VotePostCommand : IBaseRequest<VotePostResponse>
{
    public Guid PostId { get; set; }
    
    public VoteType Type { get; set; }
    
    public class Validator : AbstractValidator<VotePostCommand>
    {
        public Validator()
        {
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.Type).NotNull();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<VotePostCommand, VotePostResponse>(httpContextAccessor)
    {
        protected override async Task<VotePostResponse> HandleAsync(VotePostCommand request, CancellationToken ct)
        {
            if (!await context.Posts.AnyAsync(p => p.Id == request.PostId, ct))
                throw new DomainException(TogetherErrorCodes.Post.PostNotFound);

            var vote = await context.PostVotes
                .FirstOrDefaultAsync(v =>
                    v.PostId == request.PostId && 
                    v.CreatedById == CurrentUserClaims.Id, ct);
            
            if (vote is null)
            {
                vote = request.MapTo<PostVote>();
                vote.MarkUserCreated(CurrentUserClaims.Id);
                
                await context.PostVotes.AddAsync(vote, ct);
                
                await context.SaveChangesAsync(ct);

                Message = "Voted";

                return new VotePostResponse
                {
                    SourceId = request.PostId,
                    Value = vote.Type,
                    IsVoted = true
                };
            }
            
            if (vote.Type == request.Type)
            {
                context.PostVotes.Remove(vote);
                
                await context.SaveChangesAsync(ct);
                
                Message = "Unvoted";

                return new VotePostResponse
                {
                    SourceId = request.PostId,
                    Value = null,
                    IsVoted = false
                };
            }
            
            vote.Type = request.Type;
            vote.MarkUserModified(CurrentUserClaims.Id);
            
            context.PostVotes.Update(vote);
            
            await context.SaveChangesAsync(ct);
            
            Message = "Voted";
            
            return new VotePostResponse
            {
                SourceId = request.PostId,
                Value = vote.Type,
                IsVoted = true
            };
        }
    }
}