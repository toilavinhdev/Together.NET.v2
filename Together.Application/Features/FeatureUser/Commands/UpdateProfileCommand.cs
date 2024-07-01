namespace Together.Application.Features.FeatureUser.Commands;

public class UpdateProfileCommand : IBaseRequest
{
    public Gender Gender { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Biography { get; set; }
    
    public class Validator : AbstractValidator<UpdateProfileCommand>
    {
        public Validator()
        {
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