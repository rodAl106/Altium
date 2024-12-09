using System.Text;

namespace FileWorkerApp.Tests.TestFixtures
{
    public static class SortFileTestFixture
    {
        public static StringBuilder GetFileData()
        {

            var sb = new StringBuilder();

            for (int i = 0; i < 100; i++)
            {
                sb.AppendLine($"{i + 1}. Test_{i + 1}");
            }

            //Extra
            sb.AppendLine($"Test_Wrong");

            return sb;
        }

        public static MemoryStream GetStream()
        {

            return new MemoryStream(Encoding.UTF8.GetBytes(GetFileData().ToString()));

        }
    }
}
