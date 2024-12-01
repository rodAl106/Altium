using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWorkerApp.Managers.Interfaces
{
    public interface ISortFile
    {
        Task<bool> LoadAndSortFile();
    }
}
