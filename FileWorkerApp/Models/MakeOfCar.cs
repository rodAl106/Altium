using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FileWorkerApp.Models
{
    [ExcludeFromCodeCoverage]
    public class MakeOfCar
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
        public List<Brands> Results { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Brands
    {
        public int Make_ID { get; set; }

        private string _name;
        public string Make_Name
        {
            get => _name;
            set => _name = Regex.Replace(value, "[^0-9A-Za-z _-]", "");
        }
    }
}
