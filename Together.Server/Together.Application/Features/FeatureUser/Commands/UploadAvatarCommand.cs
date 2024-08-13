using Together.Application.Features.FeatureFile.Command;

namespace Together.Application.Features.FeatureUser.Commands;

public sealed class UploadAvatarCommand(IFormFile file) : IBaseRequest<string>
{
    private IFormFile File { get; set; } = file;
    
    public class Validator : AbstractValidator<UploadAvatarCommand>
    {
        public Validator()
        {
            RuleFor(x => x.File).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor,
        ISender sender,
        TogetherContext context,
        IRedisService redisService) 
        : BaseRequestHandler<UploadAvatarCommand, string>(httpContextAccessor)
    {
        private const string Bucket = "avatar";
        
        protected override async Task<string> HandleAsync(UploadAvatarCommand request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == CurrentUserClaims.Id, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);
            
            var oldFile = await context.Files.FirstOrDefaultAsync(f => f.Url == user.Avatar, ct);
            if (oldFile is not null) await sender.Send(new DeleteFileCommand(oldFile.PublicId), ct);
            
            var upload = await sender.Send(new UploadFileCommand(request.File, Bucket), ct);
            
            user.Avatar = upload.Data.Url;
            user.MarkModified();

            context.Users.Update(user);

            await context.SaveChangesAsync(ct);

            var cachedIdentity = await redisService.StringGetAsync<IdentityPrivilege>(
                TogetherRedisKeys.IdentityPrivilegeKey(user.Id));
            if (cachedIdentity is not null)
            {
                cachedIdentity.Avatar = user.Avatar;
                await redisService.StringSetAsync(TogetherRedisKeys.IdentityPrivilegeKey(user.Id), cachedIdentity);
            }
            
            Message = "Success";

            return user.Avatar;
        }
    }
}