using Microsoft.AspNetCore.Http;
using System.Text;

namespace InternIntelligence_Portfolio.Application.Helpers
{
    public static class FileHelpers
    {
        private static readonly Dictionary<string, string> CharacterReplacements = new()
        {
            {"\"", ""}, {"!", ""}, {"'", ""}, {"^", ""}, {"+", ""}, {"%", ""}, {"&", ""}, {"/", ""}, {"(", ""}, {")", ""},
            {"=", ""}, {"?", ""}, {"_", ""}, {" ", "-"}, {"@", ""}, {"€", ""}, {"¨", ""}, {"~", ""}, {",", ""}, {";", ""},
            {":", ""}, {".", "-"}, {"Ə", "e"}, {"ə", "e"}, {"Ö", "o"}, {"ö", "o"}, {"Ü", "u"}, {"ü", "u"}, {"ı", "i"},
            {"İ", "i"}, {"ğ", "g"}, {"Ğ", "g"}, {"æ", ""}, {"ß", ""}, {"â", "a"}, {"î", "i"}, {"ş", "s"}, {"Ş", "s"},
            {"Ç", "c"}, {"ç", "c"}, {"<", ""}, {">", ""}, {"|", ""}
        };

        public static async Task<string> CharacterRegulatoryAsync(string name)
        {
            return await Task.Run(() =>
            {
                var sb = new StringBuilder(name);
                foreach (var replacement in CharacterReplacements)
                {
                    sb.Replace(replacement.Key, replacement.Value);
                }
                return sb.ToString();
            });
        }

        public static async Task<string> RenameFileAsync(string path, string fileName, Func<string, string, Task<bool>> hasFileAsync)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string baseFileName = await CharacterRegulatoryAsync(oldName); // Clean the base name once
            string newFileName = $"{baseFileName}{extension}";

            int counter = 1;
            while (await hasFileAsync(path, newFileName))
            {
                // Append counter if file exists
                newFileName = $"{baseFileName}-{counter++}{extension}";
            }

            return newFileName;
        }

        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static void CleanupFailedUploads(List<(string fileName, string path)> uploadedFiles)
        {
            foreach (var (_, path) in uploadedFiles)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.StartsWith("image/");
        }

        public static bool IsSizeOk(this IFormFile formFile, int mb)
        {
            // Convert file length from bytes to megabytes
            double fileSizeInMB = formFile.Length / (1024.0 * 1024.0);
            return fileSizeInMB <= mb;
        }

        public static bool RestrictExtension(this IFormFile formFile, string[]? permittedExtensions = null)
        {
            permittedExtensions ??= [".jpg", ".png", ".webp"];
            var permittedSet = new HashSet<string>(permittedExtensions, StringComparer.OrdinalIgnoreCase);

            string extension = Path.GetExtension(formFile.FileName);
            return !string.IsNullOrEmpty(extension) && permittedSet.Contains(extension);
        }

        public static bool RestrictMimeTypes(this IFormFile formFile, string[]? permittedMimeTypes = null)
        {
            permittedMimeTypes ??= ["image/jpeg", "image/png", "image/gif"];
            var permittedSet = new HashSet<string>(permittedMimeTypes, StringComparer.OrdinalIgnoreCase);

            return permittedSet.Contains(formFile.ContentType);
        }
    }
}
