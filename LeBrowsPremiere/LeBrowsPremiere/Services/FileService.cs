using LeBrowsPremiere.Models;

namespace LeBrowsPremiere.Services
{
    public class FileService : IFileService
    {
		/// <summary>
		/// Creates a new file at the specified path and writes the contents of an IFormFile object to it.
		/// </summary>
		/// <param name="path">The path where the file should be created.</param>
		/// <param name="file">The IFormFile object containing the contents to write to the new file.</param>
		public void CreateFile(string path, IFormFile file)
        {
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);
        }

		/// <summary>
		/// Deletes the file at the specified path, if it exists.
		/// </summary>
		/// <param name="path">The path of the file to delete.</param>
		public void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

		/// <summary>
		/// Generates a file path and unique file name for a new product image file based on the web root folder path, 
        /// the specified images folder, and the extension of the provided IFormFile object.
		/// </summary>
		/// <param name="wwwRootFolderPath">The path to the wwwroot folder in the project.</param>
		/// <param name="imagesFolder">The name of the folder where product images are stored.</param>
		/// <param name="file">The IFormFile object containing the image data.</param>
		/// <returns>A ProductImage object containing the file path and name information for the new image file.</returns>
		public ProductImage GenerateFilePathForImage(string wwwRootFolderPath, string imagesFolder, IFormFile? file)
        {
            ProductImage productImage = new ProductImage()
            {
                FileName = Guid.NewGuid().ToString(),
                Extension = Path.GetExtension(file.FileName)
            };

            productImage.FilePath = Path.Combine(imagesFolder, productImage.FileName + productImage.Extension);

            return productImage;
        }
    }
}
