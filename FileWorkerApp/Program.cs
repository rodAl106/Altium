// See https://aka.ms/new-console-template for more information

//Set our DI
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Utils;
using Microsoft.Extensions.DependencyInjection;

var sc = new ServiceCollection()
    .AddServices()
    .BuildServiceProvider();

Console.WriteLine("Let's start...");

while (true)
{
    Console.WriteLine("Select Option");
    Console.WriteLine($"1 - Create File");
    Console.WriteLine($"2 - Load + Sort + New File Sorted");
    Console.WriteLine($"3 - Exit");
    var input = Console.ReadLine();

    if (input.Equals("1"))
    {
        Console.WriteLine($"1 ---- Create File -----");
        var managerCreateFile = sc.GetRequiredService<ICreateFile>();
        var _ = await managerCreateFile.CreateRandomDataToFile();
    }
    else if (input.Equals("2"))
    {
        Console.WriteLine($"2 ---- Load + Sort + New File Sorted ----");
        var managerSortFile = sc.GetRequiredService<ISortFile>();
        long chunkSize = 100 * 1024 * 1024; // 100MB chunks
        var _ = await managerSortFile.LoadAndSortFile(chunkSize);
    }
    else if (input.Equals("3"))
    {
        Console.WriteLine("Exit...");
        break;
    }
    else
    {
        break;
    }
}

Console.ReadLine();
