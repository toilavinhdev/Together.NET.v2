using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Commands;

public sealed class SignInCommand : IBaseRequest<SignInResponse>
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
    
    public class Validator : AbstractValidator<SignInCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().Matches(RegexPatterns.EmailRegex);
            RuleFor(x => x.Password).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        TogetherContext context, 
        IRedisService redisService,
        AppSettings appSettings) 
        : BaseRequestHandler<SignInCommand, SignInResponse>(httpContextAccessor)
    {
        protected override async Task<SignInResponse> HandleAsync(SignInCommand request, CancellationToken ct)
        {
            var user = await context.Users
                .Include(u => u.UserRoles)!
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);
            
            if (user.PasswordHash != request.Password.ToSha256())
                throw new DomainException(TogetherErrorCodes.User.IncorrectPassword);
            
            if (user.Status == UserStatus.Banned)
                throw new DomainException(TogetherErrorCodes.User.AccountHasBeenBanned);
            
            var at = JwtBearerProvider.GenerateAccessToken(appSettings.JwtTokenConfig, user);
            
            var rt = JwtBearerProvider.GenerateRefreshToken();
            
            await redisService.SetAsync(
                TogetherRedisKeys.IdentityPrivilegeKey(user.SubId), 
                user.MapTo<IdentityPrivilege>());
            
            Message = "Login successfully!";
            
            return new SignInResponse
            {
                AccessToken = at,
                RefreshToken = rt
            };
        }
    }
}