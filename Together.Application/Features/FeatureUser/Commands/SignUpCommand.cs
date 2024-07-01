using Together.Application.Features.FeatureUser.Responses;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Application.Features.FeatureUser.Commands;

public class SignUpCommand : IBaseRequest<SignUpResponse>
{
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public Gender Gender { get; set; }
    
    public string Password { get; set; } = default!;
    
    public class Validator : AbstractValidator<SignUpCommand>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty().Matches(RegexPatterns.UserNameRegex);
            RuleFor(x => x.Email).NotEmpty().Matches(RegexPatterns.EmailRegex);
            RuleFor(x => x.Gender).NotNull();
            RuleFor(x => x.Password).NotNull();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        IRedisService redisService,
        TogetherContext context) 
        : BaseRequestHandler<SignUpCommand, SignUpResponse>(httpContextAccessor)
    {
        protected override async Task<SignUpResponse> HandleAsync(SignUpCommand request, CancellationToken ct)
        {
            var userNameExists = await context.Users.AnyAsync(x => x.UserName == request.UserName, ct);
            if (userNameExists) throw new DomainException(TogetherErrorCodes.User.UserNameAlreadyExists);
            
            var emailExists = await context.Users.AnyAsync(x => x.Email == request.Email, ct);
            if (emailExists) throw new DomainException(TogetherErrorCodes.User.UserEmailAlreadyExists);
            
            var memberRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Member", ct);
            if (memberRole is null) throw new ApplicationException("Member role has not been initialized");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                Status = UserStatus.Active,
                Gender = request.Gender,
                PasswordHash = request.Password.ToSha256(),
                UserRoles = [new UserRole{ RoleId = memberRole.Id }]
            };
            
            user.MarkCreated();

            await context.Users.AddAsync(user, ct);
            
            await context.SaveChangesAsync(ct);
            
            await redisService.SetAsync(
                TogetherRedisKeys.GetIdentityPrivilegeKey(user.Id), 
                user.MapTo<IdentityPrivilege>());

            Message = "Sign up account successfully";

            return user.MapTo<SignUpResponse>();
        }
    }
}