using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

public static class FileHelper
{
    public static IFormFile CreateTestFormFile(string fileName = "test.jpg", string content = "fake image content")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "Photo", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
}

