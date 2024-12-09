using FileWorkerApp.Managers;
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Providers.Interfaces;
using FileWorkerApp.Tests.TestFixtures;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FileWorkerApp.Tests.Managers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SortFileTests
    {
        private ISortFile _manager;
        private Mock<IFileProvider> _mockFileProvider;

        private int _countFileWriter;

        [SetUp]
        public void SetUp()
        {
            _countFileWriter = 0;

            _mockFileProvider = new();

            _mockFileProvider
                .Setup(s => s.Reader(It.IsAny<string>()))
                .Returns(new StreamReader(SortFileTestFixture.GetStream()));

            _mockFileProvider
                .Setup(s => s.Reader(It.IsAny<string>(), true))
                .Returns(new StreamReader(SortFileTestFixture.GetStream()));

            _mockFileProvider
                .Setup(s => s.Writer(It.IsAny<string>(), true, Encoding.UTF8, 65536))
                .Callback(() => _countFileWriter++)
                .Returns(new StreamWriter("path"));

            _mockFileProvider
                .Setup(s => s.Writer(It.IsAny<string>(), true))
                .Returns(new StreamWriter("path_2"));

            _manager = new SortFile(_mockFileProvider.Object);
        }


        [Test]
        public async Task Test1_1() {

            //Arrange
            long chunkSize = 1024;

            //Act
            var result = await _manager.LoadAndSortFile(chunkSize);


            //Assert            
            Assert.That(result, Is.True);


        }
    }
}
