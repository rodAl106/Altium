using FileWorkerApp.Providers.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FileWorkerApp.Providers
{
    [ExcludeFromCodeCoverage]
    public class FileProvider : IFileProvider
    {
        public DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);

        public bool DeleteDirectory(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
                return true;
            }
            return false;
        }

        public StreamReader Reader(string path) => new(path);

        public StreamReader Reader(string path, bool detectEncodingFromByteOrderMarks) => new(path, detectEncodingFromByteOrderMarks);

        public StreamWriter Writer(string path, bool append, Encoding encoding, int bufferSize)
        {
            return new(path, append, encoding, bufferSize);
        }

        public StreamWriter Writer(string path, bool append) => new(path, append);
    }
}
