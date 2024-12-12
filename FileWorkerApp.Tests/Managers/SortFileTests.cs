using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Schema;
using FileWorkerApp.Managers;
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Providers.Interfaces;
using FileWorkerApp.Tests.TestFixtures;
using Moq;
using static System.Net.Mime.MediaTypeNames;

namespace FileWorkerApp.Tests.Managers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SortFileTests
    {
        private ISortFile _manager;
        private Mock<IFileProvider> _mockFileProvider;

        private int _sortAndWriteChunk_FileWriterCount, _mergeSortedChunks_FileWriterCount;
        private bool _readInputFile;
        private const string _outputFile = "sortedfile.txt";

        [SetUp]
        public void SetUp()
        {
            _sortAndWriteChunk_FileWriterCount = 0;
            _mergeSortedChunks_FileWriterCount = 0;
            _readInputFile = false;

            _mockFileProvider = new();

            _mockFileProvider
                .Setup(s => s.Reader(It.IsAny<string>()))
                .Callback((string s) =>
                {
                    if (s.Contains("input.txt"))
                        _readInputFile = true;
                    else
                        _readInputFile = false;
                })
                .Returns(new StreamReader(SortFileTestFixture.GetStream()));

            _mockFileProvider
                .Setup(s => s.Reader(It.IsAny<string>(), true))
                .Returns((string path, bool _) => {
                    var file = new FileInfo(path).Name;
                    return new StreamReader(file, true);
                });

            _mockFileProvider
                .Setup(s => s.Writer(It.IsAny<string>(), true, Encoding.UTF8, 65536))
                .Callback(() => _sortAndWriteChunk_FileWriterCount++)
                .Returns(() =>
                {
                    return new StreamWriter($"chunk_sorted_{_sortAndWriteChunk_FileWriterCount - 1}.txt");
                });

            _mockFileProvider
                .Setup(s => s.Writer(It.IsAny<string>(), true))
                .Callback(() => _mergeSortedChunks_FileWriterCount++)
                .Returns(new StreamWriter(_outputFile));

            _manager = new SortFile(_mockFileProvider.Object);
        }


        [Test]
        public async Task Test_LoadAndSortFile_If_RunsAllTheFiles()
        {
            //Arrange
            long chunkSize = 1024;

            //Act
            var result = await _manager.LoadAndSortFile(chunkSize);

            //Assert 
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(_readInputFile, Is.True);
                Assert.That(_sortAndWriteChunk_FileWriterCount, Is.GreaterThan(1));
                Assert.That(_mergeSortedChunks_FileWriterCount, Is.EqualTo(1));
            });
           
        }

        [Test]
        public async Task Test_LoadAndSortFile_TestSort() {

            //Arrange
            long chunkSize = 1024;

            //Act
            var result = await _manager.LoadAndSortFile(chunkSize);

            //Assert            
            //Validate if outputFile is sorted
            var listOutputFile = new List<(int, string)>();
            
            //Get data frim sorted file
            using (StreamReader sr = new(_outputFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)  {

                    var values = line.Split(". ");
                    if (values.Length < 2){
                        continue;
                    }

                    var num = int.Parse(values[0]);
                    var text = values[1];
                    listOutputFile.Add((num, text));
                }
            }

            //dummy list and do the order
            var listToCompare = SortFileTestFixture
                .GetList()
                .OrderBy(o => o.Item2)  //TEXT
                .ThenBy(o => o.Item1)  //NUMBER
                .ToList();

            Assert.Multiple(() => {
                Assert.That(listToCompare.First().Item1, Is.EqualTo(listOutputFile.First().Item1)); //Check the first one
                Assert.That(listToCompare.Last().Item1, Is.EqualTo(listOutputFile.Last().Item1));   //Check the last one
            });

        }
    }
}
