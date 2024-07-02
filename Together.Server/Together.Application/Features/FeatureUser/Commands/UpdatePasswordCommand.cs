namespace Together.Application.Features.FeatureUser.Commands;

public sealed class UpdatePasswordCommand : IBaseRequest
{
    public string CurrentPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;

    public string ConfirmNewPassword { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdatePasswordCommand>
    {
        public Validator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Matches(x => x.NewPassword);
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) : 
        BaseRequestHandler<UpdatePasswordCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdatePasswordCommand request, CancellationToken ct)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == CurrentUserClaims.Id, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            if (user.PasswordHash != request.CurrentPassword.ToSha256()) 
                throw new DomainException(TogetherErrorCodes.User.IncorrectPassword);

            user.PasswordHash = request.NewPassword.ToSha256();
            user.MarkModified();
            
            context.Users.Update(user);
            
            await context.SaveChangesAsync(ct);

            Message = "Done";
        }
    }
}