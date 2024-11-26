// See https://aka.ms/new-console-template for more information

//Set our DI
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Utils;
using Microsoft.Extensions.DependencyInjection;

var sc = new ServiceCollection()
    .AddServices()
    .BuildServiceProvider();


Console.WriteLine("Let's start...");

var managerCreateFile = sc.GetRequiredService<ICreateFile>();

var _ = await managerCreateFile.CreateRandomDataToFile();



Console.ReadLine();
