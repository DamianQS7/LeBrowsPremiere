using LeBrowsPremiere.Models;

namespace LeBrowsPremiere.Services
{
    public interface IFileService
    {
        void CreateFile(string path, IFormFile file);
        void DeleteFile(string path);
        ProductImage GenerateFilePathForImage(string wwwRootFolderPath, string imagesFolder, IFormFile? file);
    }
}
