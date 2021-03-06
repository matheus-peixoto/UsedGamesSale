using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace UsedGamesSale.Services.ImageFilter
{
    public class ImageHandler
    {
        public static string[] GetAllTempImageRelativePaths(string relativePath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (!Directory.Exists(path)) return new string[0];
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = $"/{relativePath}/{Path.GetFileName(files[i])}";
            }

            return files;
        }

        public static RecordResult Record(string relativePath, IFormFile img)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return PassImgToComputer(img, path, relativePath);
        }

        public static RecordResult Change(string relativeFolder, string oldRelativePath, IFormFile img)
        {
            Result result = Delete(oldRelativePath);
            RecordResult recordResult = new RecordResult() { Success = result.Success };
            if (!result.Success) return recordResult;
            recordResult = Record($"{relativeFolder}", img);
            return recordResult;
        }

        public static RecordResult MoveTempImgs(int gameId, string relativeTempImgPath, string relativePath)
        {
            RecordResult result = new RecordResult() { Paths = new List<string>() };

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
                string fileName = Path.GetFileName(img);
                File.Copy(img, $"{path}/{fileName}");
                result.Paths.Add($"/{relativePath}/{gameId}/{fileName}");
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
