using System.Text;

namespace FileWorkerApp.Tests.TestFixtures
{
    public static class SortFileTestFixture
    {
        public static StringBuilder GetFileData()
        {
            var sb = new StringBuilder();

            //Create a random list
            foreach (var item in GetList())
            {
                sb.AppendLine($"{item.Item1}. {item.Item2}");
            }

            //Extra
            sb.AppendLine($"Test_Wrong");

            return sb;
        }

        public static MemoryStream GetStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(GetFileData().ToString()));
        }

        public static List<(int, string)> GetList() {

            var list = new List<(int, string)>();

            for (int i = 0; i < 1000; i++)
            {
                var num = i + 1;
                var text = $"Test_{i + 1}";

                list.Add((num, text));
            }

            return list
                .OrderBy(o => Guid.NewGuid())
                .ToList();
        }
    }
}
