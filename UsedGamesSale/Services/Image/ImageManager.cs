using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UsedGamesSale.Services.Image
{
    public class ImageManager
    {
        public static RecordResult RecordHandler(string relativePath, IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Record(file, path, relativePath);
        }

        private static RecordResult Record(IFormFile file, string path, string relativePath)
        {
            string imgPath = Path.Combine(path, file.FileName);

            RecordResult result = new RecordResult();
            try
            {
                if (!File.Exists(imgPath))
                {
                    using (FileStream fileStream = new FileStream(imgPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                else
                    result.ErrorMessage = "This file already exist";
            }
            catch (IOException e)
            {
                result.ErrorMessage = e.Message;
            }

            result.Path = Path.Combine(relativePath, file.FileName);
            return result;
        }
    }
}
