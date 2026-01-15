using System.Threading.Tasks;

namespace Bilet1.Helpers;

public static class FileHelper
{
    public static bool CheckSize(this IFormFile file, int mb)
    {
        return file.Length < mb * 1024 * 1024;
    }

    public static bool CheckType(this IFormFile file, string type)
    {
        return file.ContentType.Contains(type);
    }

    public static async Task<string> FileUploadAsync(this IFormFile file, string folderPath)
    {
        string uniqFileName = Guid.NewGuid().ToString() + file.FileName;

        string path = Path.Combine(folderPath, uniqFileName);

        using FileStream stream = new(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return uniqFileName;
    }

    public static void FileDelete(string path)
    {
        if(File.Exists(path))
            File.Delete(path);
    }
}
