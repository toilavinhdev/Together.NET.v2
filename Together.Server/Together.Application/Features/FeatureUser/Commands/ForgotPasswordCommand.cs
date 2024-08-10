namespace Together.Application.Features.FeatureUser.Commands;

public sealed class ForgotPasswordCommand : IBaseRequest
{
    public string Email { get; set; } = default!;
    
    public class Validator : AbstractValidator<ForgotPasswordCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().Matches(RegexPatterns.EmailRegex);
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        TogetherContext context, 
        AppSettings appSettings, 
        IRedisService redisService) 
        : BaseRequestHandler<ForgotPasswordCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(ForgotPasswordCommand request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            var token = 40.RandomString();
            
            var existedToken = await redisService.StringGetAsync(TogetherRedisKeys.ForgotPasswordTokenKey(user.SubId));
            
            if (existedToken is not null)
            {
                await redisService.RemoveAsync(TogetherRedisKeys.ForgotPasswordTokenKey(user.SubId));
            }

            await redisService.StringSetAsync(
                TogetherRedisKeys.ForgotPasswordTokenKey(user.SubId), 
                token,
                TimeSpan.FromHours(TogetherBusinessConfigs.ForgotPasswordDurationInHours));
            
            // Thread Pool
            await Task.Run(() =>
            {
                _ = MailProvider.SmtpSendAsync(appSettings.MailConfig, new MailForm
                {
                    To = request.Email,
                    Title = "TOGETHER.NET RESET PASSWORD",
                    Body = "forgot-password-template.html"
                        .ToTemplatePath()
                        .ReadAllText()
                        .Replace("{{user_name}}", user.UserName)
                        .Replace("{{validate_url}}", new UriBuilder($"{appSettings.Host}/auth/forgot-password/submit/{user.SubId}/{token}").Uri.ToString())
                        .Replace("{{duration_in_hours}}", TogetherBusinessConfigs.ForgotPasswordDurationInHours.ToString())
                        .Replace("{{contact_email}}", appSettings.MailConfig.Mail)
                });
            }, ct);

            Message = "Success";
        }
    }
}