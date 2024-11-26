using FileWorkerApp.Managers;
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Providers;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace FileWorkerApp.Utils
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {

            RegisterMangers(serviceCollection);
            RegisterProviders(serviceCollection);

            return serviceCollection;
        }

        private static void RegisterMangers(IServiceCollection serviceCollection)
        {

            serviceCollection.AddTransient<ICreateFile, CreateFile>();
        }


        private static void RegisterProviders(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddRefitClient<ICarsApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://vpic.nhtsa.dot.gov/api");
                });
        }
    }
}
