namespace ECommerce.Application.Interfaces.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName);
    }
}
