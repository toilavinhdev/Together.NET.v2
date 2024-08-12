using System.Security.Authentication;
using Together.Application.Features.FeatureUser.Responses;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Application.Features.FeatureUser.Commands;

public sealed class ExternalAuthCommand : IBaseRequest<SignInResponse>
{
    public string Credential { get; set; } = default!;
    
    public class Validator : AbstractValidator<ExternalAuthCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Credential).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        TogetherContext context, 
        AppSettings appSettings,
        IRedisService redisService) 
        : BaseRequestHandler<ExternalAuthCommand, SignInResponse>(httpContextAccessor)
    {
        protected override async Task<SignInResponse> HandleAsync(ExternalAuthCommand request, CancellationToken ct)
        {
            var payload = await OAuthProvider.VerifyGoogleCredential(
                appSettings.GoogleOAuthConfig.ClientId, 
                request.Credential);
            if (payload is null) throw new InvalidCredentialException();
            
            var user = await context.Users
                .Include(u => u.UserRoles)!
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == payload.Email, ct);
            
            if (user is null)
            {
                var memberRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Member", ct);
                if (memberRole is null) throw new ApplicationException("Member role has not been initialized");

                var userName = await RandomUserNameAsync();

                user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = payload.Name,
                    UserName = userName,
                    Email = payload.Email,
                    PasswordHash = 12.RandomString().ToSha256(),
                    UserRoles = [
                        new UserRole
                        {
                            RoleId = memberRole.Id
                        }
                    ],
                    Avatar = payload.Picture,
                    Status = UserStatus.Active,
                    Gender = Gender.Other
                };
                user.MarkCreated();
                
                await context.Users.AddAsync(user, ct);
                
                await context.SaveChangesAsync(ct);
            }
            
            var at = JwtBearerProvider.GenerateAccessToken(appSettings.JwtTokenConfig, user);

            var rt = JwtBearerProvider.GenerateRefreshToken();
            
            await redisService.StringSetAsync(
                TogetherRedisKeys.IdentityPrivilegeKey(user.Id), 
                user.MapTo<IdentityPrivilege>());
            
            Message = "External login successfully!";
            
            return new SignInResponse
            {
                AccessToken = at,
                RefreshToken = rt
            };
        }

        private async Task<string> RandomUserNameAsync()
        {
            while (true)
            {
                var random = 24.RandomString().ToLower();
                if (await context.Users.AnyAsync(u => u.UserName == random)) continue;
                return random;
            }
        }
    }
}