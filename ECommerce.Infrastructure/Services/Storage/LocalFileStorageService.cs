using ECommerce.Application.Interfaces.Storage;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            var filePath = Path.Combine(rootPath, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await fileStream.CopyToAsync(stream);

            return $"{folderName}/{uniqueFileName}";
        }
    }
}
