namespace Together.Application.Features.FeatureUser.Commands;

public sealed class LogoutCommand : IBaseRequest
{
    internal class Handler(IHttpContextAccessor httpContextAccessor) 
        : BaseRequestHandler<LogoutCommand>(httpContextAccessor)
    {
        protected override Task HandleAsync(LogoutCommand request, CancellationToken ct)
        {
            Message = "Logout!";
            
            return Task.CompletedTask;
        }
    }
}