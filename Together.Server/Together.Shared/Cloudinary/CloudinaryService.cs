using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Together.Shared.ValueObjects;

namespace Together.Shared.Cloudinary;
using Cloudinary = CloudinaryDotNet.Cloudinary;

public interface ICloudinaryService
{
    Task<ImageUploadResult?> UploadImageAsync(Stream stream, string fileId, string? bucket);

    Task<VideoUploadResult?> UploadVideoAsync(Stream stream, string publicId, string? bucket);

    Task<DeletionResult> DeleteAsync(string publicId);
}

public class CloudinaryService(AppSettings appSettings) : ICloudinaryService
{
    private readonly Cloudinary _cloudinary = new(appSettings.CloudinaryUrl)
    {
        Api =
        {
            Secure = true
        }
    };
    
    public async Task<ImageUploadResult?> UploadImageAsync(Stream stream, string fileId, string? bucket)
    {
        var parameters = new ImageUploadParams
        {
            File = new FileDescription(fileId, stream),
            Folder = bucket,
            PublicId = fileId
        };

        var result = await _cloudinary.UploadAsync(parameters);

        return result;
    }

    public async Task<VideoUploadResult?> UploadVideoAsync(Stream stream, string publicId, string? bucket)
    {
        var parameters = new VideoUploadParams
        {
            File = new FileDescription(publicId, stream),
            Folder = bucket,
            PublicId = publicId
        };

        var result = await _cloudinary.UploadAsync(parameters);

        return result;
    }

    public async Task<DeletionResult> DeleteAsync(string publicId)
    {
        var parameters = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(parameters);
        
        return result;
    }
}