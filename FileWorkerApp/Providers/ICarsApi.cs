using FileWorkerApp.Models;
using Refit;

namespace FileWorkerApp.Providers
{
    public interface ICarsApi
    {
        [Get("/vehicles/getallmakes?format=json")]
        Task<MakeOfCar> GetMakeOfCars();

        [Get("/vehicles/getallmanufacturers?format=json")]
        Task<CarManufacturer> GetCarManufacturers();
    }
}
