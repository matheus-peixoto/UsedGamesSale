using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.Image
{
    public class ImageHandler
    {
        public static string[] GetAllTempImageRelativePaths(string relativePath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = $"/{relativePath}/{Path.GetFileName(files[i])}";
            }

            return files;
        }

        public static RecordResult Record(string relativePath, IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return PassImgToComputer(file, path, relativePath);
        }

        public static Result MoveTempImgs(int gameId, string relativeTempImgPath, string relativePath)
        {
            Result result = new Result();

            string tempImgsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeTempImgPath);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath, gameId.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(tempImgsPath))
            {
                result.ErrorMessage = "Temp folder does not exists";
                return result;
            }

            string[] imgs = Directory.GetFiles(tempImgsPath);
            foreach (string img in imgs)
            {
                File.Copy(img, $"{path}/{Path.GetFileName(img)}");
            }

            result.Success = true;
            return result;
        }

        public static Result Delete(string relativeImgPath)
        {
            Result result = new Result();
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeImgPath.TrimStart('/'));
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
