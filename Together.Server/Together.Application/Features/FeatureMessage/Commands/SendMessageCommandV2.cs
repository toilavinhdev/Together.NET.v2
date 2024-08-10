using Together.Application.Features.FeatureMessage.Responses;
using Together.Application.Sockets;

namespace Together.Application.Features.FeatureMessage.Commands;

public sealed class SendMessageCommandV2 : IBaseRequest<SendMessageResponse>
{
    public Guid ConversationId { get; set; }

    public string Text { get; set; } = default!;
    
    public class Validator : AbstractValidator<SendMessageCommandV2>
    {
        public Validator()
        {
            RuleFor(x => x.ConversationId).NotEmpty();
            RuleFor(x => x.Text).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, IRedisService redisService, TogetherWebSocketHandler socket)
        : BaseRequestHandler<SendMessageCommand, SendMessageResponse>(httpContextAccessor)
    {
        protected override Task<SendMessageResponse> HandleAsync(SendMessageCommand request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}