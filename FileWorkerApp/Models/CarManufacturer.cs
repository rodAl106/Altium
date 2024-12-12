using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FileWorkerApp.Models
{
    [ExcludeFromCodeCoverage]
    public class CarManufacturer
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }

        public List<Manufacturer> Results { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Manufacturer
    {
        public int Mfr_ID { get; set; }

        private string _name;
        public string Mfr_Name
        {
            get => _name;
            set => _name = Regex.Replace(value, "[^0-9A-Za-z _-]", "");
        }
    }
}
