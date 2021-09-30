using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CheckIfExistsDistinctList
{
    class Program
    {
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string NewFileName { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var _config = builder.Build();

            Console.WriteLine("AppSettings Configured");

            IFileChecker fileChecker = new FileChecker(_config);
            //System.Threading.Tasks.Task task =
            //fileChecker.RunFileCheckAndCreate();
            fileChecker.RunFileCheckKeywordAndCreate();

            Console.ReadKey();
        }
    }
}
