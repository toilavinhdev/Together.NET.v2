namespace Together.Application.Features.FeatureUser.Commands;

public sealed class RefreshTokenCommand : IBaseRequest<TokenValue>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor) 
        : BaseRequestHandler<RefreshTokenCommand, TokenValue>(httpContextAccessor)
    {
        protected override async Task<TokenValue> HandleAsync(RefreshTokenCommand request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}