using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Tests.Common
{
    public static class Utilities
    {
        public static FormFile GenerateMockFile(string fileName, string contentType, int fileSizeBytes)
        {
            var fileContent = new byte[fileSizeBytes]; // Simulating file size
            var stream = new MemoryStream(fileContent);

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
