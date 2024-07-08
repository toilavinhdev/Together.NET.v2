namespace Together.Application.Features.FeatureUser.Commands;

public sealed class UpdateProfileCommand : IBaseRequest
{
    public string UserName { get; set; } = default!;
    
    public Gender Gender { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Biography { get; set; }
    
    public class Validator : AbstractValidator<UpdateProfileCommand>
    {
        public Validator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Matches(RegexPatterns.UserNameRegex);
            RuleFor(x => x.Gender).NotNull();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) :
        BaseRequestHandler<UpdateProfileCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdateProfileCommand request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == CurrentUserClaims.Id, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            if (request.UserName != user.UserName && await context.Users.AnyAsync(u => u.UserName == request.UserName, ct))
                throw new DomainException(TogetherErrorCodes.User.UserNameAlreadyExists);
            
            user.UserName = request.UserName;
            user.Gender = request.Gender;
            user.FullName = request.FullName;
            user.Biography = request.Biography;
            user.MarkModified();
            
            context.Users.Update(user);
            
            await context.SaveChangesAsync(ct);

            Message = "Done";
        }
    }
}