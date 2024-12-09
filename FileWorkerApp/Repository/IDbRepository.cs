namespace FileWorkerApp.Repository
{
    public interface IDbRepository<T> where T : class
    {
        Task Create(T toAdd);

    }
}
