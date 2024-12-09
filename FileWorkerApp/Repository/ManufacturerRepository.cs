using FileWorkerApp.Models.DB;
using FileWorkerApp.Providers;

namespace FileWorkerApp.Repository
{
    public class ManufacturerRepository : IDbRepository<Manufacturer>
    {
        private readonly DatabaseInMemory _contex;

        public ManufacturerRepository(DatabaseInMemory contex)
        {
            _contex = contex;
        }

        public async Task Create(Manufacturer toAdd)
        {
            using (var conn = _contex)
            {

                conn.Manufacturers.Add(toAdd);
                await conn.SaveChangesAsync();
            }
        }
    }
}
