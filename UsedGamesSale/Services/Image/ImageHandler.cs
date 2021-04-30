using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UsedGamesSale.Services.Image
{
    public class ImageHandler
    {
        public static RecordResult Record(string relativePath, IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return PassImgToComputer(file, path, relativePath);
        }

        public static Result Delete(string imgPath)
        {
            Result result = new Result();

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPath.TrimStart('/'));
            if (!File.Exists(fullPath))
            {
                result.ErrorMessage = "File does not exist";
                return result;
            }

            File.Delete(fullPath);
            result.Success = true;
            return result;
        }

        public static Result DeleteImgFolder(string relativePath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            Result result = new Result();
            if (!Directory.Exists(path))
            {
                result.ErrorMessage = "Directory does not exist";
                return result;
            }

            string[] imgs = Directory.GetFiles(path);
            foreach (string img in imgs)
            {
                File.Delete(img);
            }
            Directory.Delete(path);
            result.Success = true;

            return result;
        }

        private static RecordResult PassImgToComputer(IFormFile file, string path, string relativePath)
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

                    result.Success = true;
                    result.Path = $"/{relativePath}/{file.FileName}";
                }
                else
                    result.ErrorMessage = "This file already exist";

            }
            catch (IOException e)
            {
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}
