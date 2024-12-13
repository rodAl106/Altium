using System.Text;
using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Providers.Interfaces;

namespace FileWorkerApp.Managers
{
    public class SortFile : ISortFile
    {
        private readonly IFileProvider _fileProvider;

        public SortFile(IFileProvider fileProvider) => _fileProvider = fileProvider;

        private const string path = @"..\..\..\..\";
        private const string fileName = "input.txt";

        public async Task<bool> LoadAndSortFile(long chunkSize)
        {
            string inputFile = $"{path}{fileName}";
            string tempDir = $"{path}temp_chunks";
            string outputFile = "sortedfile.txt";

            //Step 1 - Split and sort chunks
            _fileProvider.CreateDirectory(tempDir);

            //long chunkSize = 100 * 1024 * 1024; // 100MB chunks
            Console.WriteLine("Splitting and sorting chunks...");
            var tempFiles = await SplitAndSortChunks(inputFile, tempDir, chunkSize);

            //Step 2 - Merge
            Console.WriteLine("Merging sorted chunks...");
            MergeSortedChunks(tempFiles, outputFile);

            // Cleanup
            _fileProvider.DeleteDirectory(tempDir, true);
            Console.WriteLine("Sorting complete! Sorted file: " + outputFile);

            return true;
        }

        private async Task<List<string>> SplitAndSortChunks(string inputFile, string tempDir, long chunkSize)
        {
            List<string> tempFiles = [];
            List<(int, string)> lines = [];

            long currentSize = 0;
            int fileCounter = 0;

            var start = DateTime.Now;
            double totalMinutes = 0;
            Console.WriteLine($"Start {start}");

            using (StreamReader reader = _fileProvider.Reader(inputFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //Split the line
                    var values = line.Split(". ");
                    if (values.Length < 2)
                        continue;

                    var num = int.Parse(values[0]);
                    var text = values[1];

                    lines.Add((num, text));
                    currentSize += line.Length + Environment.NewLine.Length;

                    // When the chunk reaches the specified size, sort and write it
                    if (currentSize >= chunkSize)
                    {
                        string tempFile = await SortAndWriteChunk(lines, tempDir, fileCounter++);

                        //To check time
                        //if (fileCounter % 10 == 0)
                        //{
                        //    var end = DateTime.Now;
                        //    totalMinutes = (end - start).TotalMinutes - totalMinutes;
                        //    Console.WriteLine($"Time per 1Gb - {totalMinutes}");
                        //}

                        tempFiles.Add(tempFile);
                        lines.Clear();
                        currentSize = 0;
                    }
                }
            }

            return tempFiles;
        }

        private async Task<string> SortAndWriteChunk(List<(int number, string text)> lines, string tempDir, int fileCounter)
        {
            var sortedList = lines
                .OrderBy(o => o.text)
                .ThenBy(o => o.number);

            // Write sorted records to a temporary file
            string tempFile = Path.Combine(tempDir, $"chunk_sorted_{fileCounter}.txt");

            using (StreamWriter streamwriter = _fileProvider.Writer(tempFile, true, Encoding.UTF8, 65536))
            {
                foreach (var (number, text) in sortedList)
                {
                    await streamwriter.WriteLineAsync($"{number}. {text}");
                }

                streamwriter.Dispose();
                streamwriter.Close();
            }

            return tempFile;
        }

        private bool MergeSortedChunks(List<string> tempFiles, string outputFile)
        {
            var readers = tempFiles
                .Select(s => _fileProvider.Reader(s, true))
                .ToList();

            var priorityQueue = new SortedDictionary<string, Queue<(int fileIndex, string line)>>(StringComparer.Ordinal);

            try
            {
                // Initialize priority queue with the first line from each file
                for (int i = 0; i < readers.Count; i++)
                {
                    string line = readers[i].ReadLine();
                    if (line != null)
                    {
                        AddToPriorityQueue(priorityQueue, i, line);
                    }
                }

                using (StreamWriter writer = _fileProvider.Writer(outputFile, true))
                {
                    while (priorityQueue.Count > 0)
                    {
                        // Get the smallest element
                        var smallestEntry = priorityQueue.First();
                        var (fileIndex, line) = smallestEntry.Value.Dequeue();

                        // Write the smallest element to the output file
                        writer.WriteLine(line);

                        // Remove the queue if it's empty
                        if (smallestEntry.Value.Count == 0)
                        {
                            priorityQueue.Remove(smallestEntry.Key);
                        }

                        // Read the next line from the file
                        string nextLine = readers[fileIndex].ReadLine();
                        if (nextLine != null)
                        {
                            AddToPriorityQueue(priorityQueue, fileIndex, nextLine);
                        }
                    }
                }
            }
            finally
            {
                // Clean up readers
                foreach (var reader in readers)
                {
                    reader.Dispose();
                }
            }

            return true;
        }

        private bool AddToPriorityQueue(SortedDictionary<string, Queue<(int, string)>> priorityQueue, int fileIndex, string line)
        {
            // Parse the line to extract the text portion for sorting
            var values = line.Split(". ");
            if (values.Length < 2)
                return false;

            string text = values[1];

            if (!priorityQueue.ContainsKey(text))
            {
                priorityQueue[text] = new Queue<(int, string)>();
            }

            priorityQueue[text].Enqueue((fileIndex, line));

            return true;
        }

    }
}
