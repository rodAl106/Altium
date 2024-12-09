namespace FileWorkerApp.Managers.Interfaces
{
    public interface ISortFile
    {
        Task<bool> LoadAndSortFile(long chunkSize);
    }
}
