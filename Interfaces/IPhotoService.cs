using CloudinaryDotNet.Actions;

namespace HouseholdOnlineStore.Interfaces
{
    public interface IPhotoService
    {
        ImageUploadResult AddPhoto(IFormFile file);

        DeletionResult DeletePhoto(string publicId);
    }
}
