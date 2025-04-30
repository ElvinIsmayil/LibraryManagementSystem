using Library_Management_System.Models;
using Library_Management_System.ViewModels.Author;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Library_Management_System.Extensions
{
    public static class ImageExtension
    {
        public static void FileTypeCheck(this IFormFile file, ModelStateDictionary modelState)
        {
            if (file is not null)
            {

                if (!file.ContentType.StartsWith("image/"))
                {
                    modelState.AddModelError("Image", "You can only upload image files!");
                }

                const int MaxFileSizeInMB = 2;
                if (file.Length > MaxFileSizeInMB * 1024 * 1024)
                {
                    modelState.AddModelError("Image", $"The image file size should not be larger than {MaxFileSizeInMB} MB");
                }
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    modelState.AddModelError("Image", "Only .jpg, .jpeg, and .png files are allowed.");
                }
            }

        }
        public static async Task<String> SaveImage(this IFormFile file,string folderName)
        {
            if (file is not null)
            {
                string fileName = Guid.NewGuid().ToString() + file.FileName;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                string imageUrl = $"/uploads/{folderName}/" + fileName;
                return imageUrl;
            }
            else
            {
                return "";
            }

        }

        public static void DeleteImageFromLocal(this IFormFile file)
        {
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "authors", file.FileName);

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            
        }






    }
}
