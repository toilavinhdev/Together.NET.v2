using Together.Shared.Cloudinary;

namespace Together.Application.Features.FeatureFile.Command;

public sealed class DeleteFileCommand(string publicId) : IBaseRequest
{
    private string PublicId { get; set; } = publicId;
    
    public class Validator : AbstractValidator<DeleteFileCommand>
    {
        public Validator()
        {
            RuleFor(x => x.PublicId).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        TogetherContext context, 
        ICloudinaryService cloudinaryService) 
        : BaseRequestHandler<DeleteFileCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeleteFileCommand request, CancellationToken ct)
        {
            var file = await context.Files.FirstOrDefaultAsync(f => f.PublicId == request.PublicId, ct);
            if (file is null) return;

            context.Files.Remove(file);

            await cloudinaryService.DeleteAsync(file.PublicId);

            await context.SaveChangesAsync(ct);
        }
    }
}