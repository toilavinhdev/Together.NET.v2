using Together.Application.Features.FeatureReply.Responses;
using Together.Domain.Aggregates.ReplyAggregate;

namespace Together.Application.Features.FeatureReply.Commands;

public sealed class VoteReplyCommand : IBaseRequest<VoteReplyResponse>
{
    public Guid ReplyId { get; set; }
    
    public VoteType Type { get; set; }
    
    public class Validator : AbstractValidator<VoteReplyCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ReplyId).NotEmpty();
            RuleFor(x => x.Type).NotNull();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<VoteReplyCommand, VoteReplyResponse>(httpContextAccessor)
    {
        protected override async Task<VoteReplyResponse> HandleAsync(VoteReplyCommand request, CancellationToken ct)
        {
            if (!await context.Replies.AnyAsync(p => p.Id == request.ReplyId, ct))
                throw new DomainException(TogetherErrorCodes.Reply.ReplyNotFound);

            var vote = await context.ReplyVotes
                .FirstOrDefaultAsync(v => 
                    v.ReplyId == request.ReplyId && 
                    v.CreatedById == CurrentUserClaims.Id, ct);
            
            if (vote is null)
            {
                vote = request.MapTo<ReplyVote>();
                vote.MarkUserCreated(CurrentUserClaims.Id);
                
                await context.ReplyVotes.AddAsync(vote, ct);
                
                await context.SaveChangesAsync(ct);

                Message = "Voted";

                return new VoteReplyResponse
                {
                    SourceId = request.ReplyId,
                    Value = vote.Type,
                    IsVoted = true
                };
            }
            
            if (vote.Type == request.Type)
            {
                context.ReplyVotes.Remove(vote);
                
                await context.SaveChangesAsync(ct);
                
                Message = "Unvoted";

                return new VoteReplyResponse
                {
                    SourceId = request.ReplyId,
                    Value = null,
                    IsVoted = false
                };
            }
            
            vote.Type = request.Type;
            vote.MarkUserModified(CurrentUserClaims.Id);
            
            context.ReplyVotes.Update(vote);
            
            await context.SaveChangesAsync(ct);
            
            Message = "Voted";
            
            return new VoteReplyResponse
            {
                SourceId = request.ReplyId,
                Value = vote.Type,
                IsVoted = true
            };
        }
    }
}