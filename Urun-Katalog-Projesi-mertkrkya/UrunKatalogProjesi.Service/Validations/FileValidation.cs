using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Service.Validations
{
    public static class FileValidation
    {
        public static string ImageValidation(IFormFile file, int maxFileSize)
        {
            var allowFileSize = maxFileSize * 1024;
            if (file == null)
                return "Image cannot be null";
            if (file.Length > allowFileSize)
                return $"Image cannot be larger than {maxFileSize} KB";
            var fileType = Path.GetExtension(file.FileName);
            if (fileType.ToLower() != ".jpg" && fileType.ToLower() != ".png" && fileType.ToLower() != ".jpeg")
                return $"Cannot support {fileType} file type.";
            return "";

        }
    }
}
