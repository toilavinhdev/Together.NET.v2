using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Commands;

public sealed class RefreshTokenCommand : IBaseRequest<RefreshTokenResponse>
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
    
    public class Validator : AbstractValidator<RefreshTokenCommand>
    {
        public Validator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, IRedisService redisService, AppSettings appSettings) 
        : BaseRequestHandler<RefreshTokenCommand, RefreshTokenResponse>(httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override async Task<RefreshTokenResponse> HandleAsync(RefreshTokenCommand request, CancellationToken ct)
        {
            // var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
            // var expiredAccessToken = cookies?.FirstOrDefault(pair => pair.Key == "ACCESS_TOKEN").Value;
            // var refreshToken = cookies?.FirstOrDefault(pair => pair.Key == "REFRESH_TOKEN").Value;
            // if (expiredAccessToken is null || refreshToken is null) throw new DomainException(TogetherErrorCodes.User.RefreshTokenFailed, "Cookie null");
            
            var claims = JwtBearerProvider.DecodeAccessToken(request.AccessToken);
            
            var user = await redisService.StringGetAsync<IdentityPrivilege>(TogetherRedisKeys.IdentityPrivilegeKey(claims.Id));
            if (user is null) throw new DomainException(TogetherErrorCodes.User.RefreshTokenFailed, "User not found"); 
            
            // RT hết hạn(TTL Redis = 0) hoặc không khớp
            var existedRefreshToken = await redisService.StringGetAsync(TogetherRedisKeys.RefreshTokenKey(claims.Id));
            if (string.IsNullOrEmpty(existedRefreshToken) || request.RefreshToken != existedRefreshToken) 
                throw new DomainException(TogetherErrorCodes.User.RefreshTokenFailed, "Invalid refresh token");
            
            var newAccessToken = JwtBearerProvider.GenerateAccessToken(appSettings.JwtTokenConfig, user);

            var newRefreshToken = JwtBearerProvider.GenerateRefreshToken();
            
            // TTL = Duration
            await redisService.StringSetAsync(
                TogetherRedisKeys.RefreshTokenKey(user.Id), 
                newRefreshToken,
                TimeSpan.FromDays(appSettings.JwtTokenConfig.RefreshTokenDurationInDays));

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}