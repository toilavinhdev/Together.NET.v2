using Together.Shared.Cloudinary;
using File = Together.Domain.Aggregates.FileAggregate.File;

namespace Together.Application.Features.FeatureFile.Command;

public sealed class UploadFileCommand(IFormFile file, string? bucket) : IBaseRequest<File>
{
    private IFormFile File { get; set; } = file;

    public string? Bucket { get; set; } = bucket;
    
    public class Validator : AbstractValidator<UploadFileCommand>
    {
        public Validator()
        {
            RuleFor(x => x.File).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, 
        ICloudinaryService cloudinaryService, 
        TogetherContext context) 
        : BaseRequestHandler<UploadFileCommand, File>(httpContextAccessor)
    {
        protected override async Task<File> HandleAsync(UploadFileCommand request, CancellationToken ct)
        {
            var fileId = Guid.NewGuid();

            await using var stream = request.File.OpenReadStream();
            var upload = await cloudinaryService.UploadImageAsync(stream, fileId.ToString(), request.Bucket);
            if (upload is null) throw new DomainException(TogetherErrorCodes.File.UploadFailed);

            var file = new File
            {
                Id = fileId,
                PublicId = upload.PublicId,
                DisplayName = upload.DisplayName,
                OriginalName = request.File.FileName,
                Url = upload.Url.ToString(),
                Format = upload.Format
            };
            file.MarkUserCreated(CurrentUserClaims.Id);

            await context.Files.AddAsync(file, ct);

            await context.SaveChangesAsync(ct);

            Message = "Uploaded";

            return file;
        }
    }
}