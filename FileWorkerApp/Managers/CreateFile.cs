using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Providers;
using FileWorkerApp.Utils;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Text;

namespace FileWorkerApp.Managers
{
    public class CreateFile : ICreateFile
    {
        private readonly ICarsApi _carsApi;
        private const string pathInputFile = @"..\..\..\..\input.txt";

        public CreateFile(ICarsApi carsApi) => _carsApi = carsApi;

        public async Task<bool> CreateRandomDataToFile()
        {
            var resultMake = await _carsApi.GetMakeOfCars();
            var resultManufacturer = await _carsApi.GetCarManufacturers();

            StringBuilder sb = new();
            FileInfo infoFile = new(pathInputFile);
            FileInfo infoFile2;
            Random rnd = new();

            do {

                //Thread.Sleep(1000);
                var millisecond = DateTime.Now.Millisecond;

                foreach (var brand in resultMake.Results.OrderBy(o => Guid.NewGuid()))
                {
                    sb.AppendLine($"{brand.Make_ID}. {brand.Make_Name}");
                    sb.AppendLine($"{brand.Make_ID + millisecond}. {brand.Make_Name} {millisecond}");
                    sb.AppendLine($"{brand.Make_ID + millisecond}. {brand.Make_Name} {millisecond}");
                    sb.AppendLine($"{brand.Make_ID + (millisecond * 2)}. {brand.Make_Name} {millisecond}");
                    //sb.AppendLine($"{brand.Make_ID}. {brand.Make_Name} {resultManufacturer.Results[rnd.Next(resultManufacturer.Results.Count -1)]}");
                }

                foreach (var manuf in resultManufacturer.Results.OrderBy(o => Guid.NewGuid()))
                {
                    sb.AppendLine($"{manuf.Mfr_ID}. {manuf.Mfr_Name}");
                    sb.AppendLine($"{manuf.Mfr_ID + millisecond}. {manuf.Mfr_Name} {millisecond}");
                    sb.AppendLine($"{manuf.Mfr_ID + millisecond}. {manuf.Mfr_Name} {millisecond}");
                    sb.AppendLine($"{manuf.Mfr_ID + (millisecond * 2)}. {manuf.Mfr_Name} {millisecond}");
                    //sb.AppendLine($"{manuf.Mfr_ID}. {manuf.Mfr_Name} {resultMake.Results[rnd.Next(resultMake.Results.Count - 1)]}");
                }

                if (!IsFileInUseGeneric(infoFile))
                {
                    Console.WriteLine($"**** File is not blocked *** {DateTime.Now.ToString()}");
                    Thread.Sleep(2000);
                    await File.AppendAllTextAsync(pathInputFile, sb.ToString());
                    sb.Clear();
                }
                else
                    Console.WriteLine($"**** File is blocked *** {DateTime.Now.ToShortTimeString}");

                infoFile2 = new FileInfo(pathInputFile);

            } while (BytesConverter.ConvertBytesToGB(infoFile2.Length) <= 50);


            return false;
        }


        public bool IsFileInUseGeneric(FileInfo file)
        {
            try
            {
                using var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
