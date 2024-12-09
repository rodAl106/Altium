using System.Text;

namespace FileWorkerApp.Providers.Interfaces
{
    public interface IFileProvider
    {
        DirectoryInfo CreateDirectory(string path);

        bool DeleteDirectory(string path, bool recursive);

        StreamReader Reader(string path);

        StreamReader Reader(string path, bool detectEncodingFromByteOrderMarks);

        StreamWriter Writer(string path, bool append, Encoding encoding, int bufferSize);

        StreamWriter Writer(string path, bool append) => new(path, append);
    }
}
