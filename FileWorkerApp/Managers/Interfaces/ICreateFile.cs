namespace FileWorkerApp.Managers.Interfaces
{
    public interface ICreateFile
    {
        Task<bool> CreateRandomDataToFile();
    }
}
