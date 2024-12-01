using FileWorkerApp.Managers.Interfaces;
using FileWorkerApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.OpenApi.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileWorkerApp.Managers
{
    public class SortFile : ISortFile
    {
        private const string pathInputFile = @"..\..\..\..\input.txt";

        public async Task<bool> LoadAndSortFile()
        {
            var list = new List<Brands>();

            using (StreamReader reader = new(pathInputFile))
            {
                var buffer = new char[(1024 * 1024)*50].AsSpan();

                int numberRead;
                //var stringBuilder = new StringBuilder();
                var start = DateTime.Now;
                Console.WriteLine($"Add to the list {start}");

                while ((numberRead = reader.ReadBlock(buffer)) > 0) {

                    //stringBuilder.Append(/*buffer[..numberRead]*/);

                    var text = buffer[..numberRead]
                        .ToString()
                        .Split("\r\n")
                        .Select(s => {
                            var t = s.Split(". ");
                            if (t.Length == 2)
                            {   _ = Int32.TryParse(t[0], out int result);
                                return new Brands
                                {
                                    Make_ID = result,
                                    Make_Name = t[1],
                                };
                            }
                            else
                                return null;
                        });

                    
                    list.AddRange(text);
                    //Console.WriteLine($"Add to the list({list.Count}) {DateTime.Now}");
                }
                var end = DateTime.Now;
                Console.WriteLine($"Add to the list {end} - Total: {(end - start).TotalSeconds}" );

            }



            return true;
        }
    }
}
