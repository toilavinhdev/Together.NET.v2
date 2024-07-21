namespace Together.Application.Features.FeatureUser.Commands;

public sealed class SubmitForgotPasswordTokenCommand : IBaseRequest
{
    public long UserId { get; set; }
    
    public string Token { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
    
    public class Validator : AbstractValidator<SubmitForgotPasswordTokenCommand>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, IRedisService redisService, TogetherContext context) 
        : BaseRequestHandler<SubmitForgotPasswordTokenCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(SubmitForgotPasswordTokenCommand request, CancellationToken ct)
        {
            var existedToken = await redisService.GetAsync(TogetherRedisKeys.ForgotPasswordTokenKey(request.UserId));

            if (existedToken is null || existedToken != request.Token)
                throw new DomainException(TogetherErrorCodes.User.ForgotPasswordTokenInvalid);
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.SubId == request.UserId, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            user.PasswordHash = request.NewPassword.ToSha256();
            user.MarkModified();

            context.Users.Update(user);

            await context.SaveChangesAsync(ct);

            await redisService.RemoveAsync(TogetherRedisKeys.ForgotPasswordTokenKey(request.UserId));

            Message = "Success";
        }
    }
}