using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIfExistsDistinctList
{
    public class FileChecker : IFileChecker
    {
        readonly IConfiguration _config;
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string NewFileName { get; set; }

        public FileChecker(IConfiguration config)
        {
            _config = config;

            SetPropertyValues();
        }

        // Set the Property with Appsettings.json Values
        private void SetPropertyValues()
        {
            FolderPath = _config.GetSection("Parameters:FolderPath").Value;
            FileName = _config.GetSection("Parameters:FileName").Value;
            NewFileName = _config.GetSection("Parameters:NewFileName").Value;
        }

        // Check each File Line and take out all duplicates
        // Creating a new file containing each Distinct Line
        public async Task RunFileCheckAndCreate()
        {
            string filePath = Path.Combine(FolderPath, FileName);
            string newFile = Path.Combine(FolderPath, NewFileName);


            if (File.Exists(filePath))
            {
                try
                {

                    //string[] lstFileLines = await File.ReadAllLinesAsync(filePath);
                    string[] lstFileLines =  File.ReadAllLines(filePath);

                    if (lstFileLines.Any())
                    {
                        if (!File.Exists(newFile))
                        {
                            File.Create(newFile);
                        }

                        using (StreamWriter writer = new StreamWriter(newFile, true))
                        {
                            foreach (var line in lstFileLines.Distinct())
                            {
                                await writer.WriteLineAsync(line);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!File.Exists(newFile))
                    {
                        File.Create(newFile);
                    }

                    using (StreamWriter writer = new StreamWriter(newFile, true))
                    {
                        writer.WriteLine(DateTime.Now + " - EX - " + ex);
                    }
                }
            }
            else
            {
                try
                {
                    if (!File.Exists(newFile))
                    {
                        File.Create(newFile);
                    }

                    using (StreamWriter writer = new StreamWriter(newFile, true))
                    {
                        writer.WriteLine(DateTime.Now + " - File Reading Error, Does NOT Exist");
                    }
                }
                catch (Exception ex)
                {
                    if (!File.Exists(newFile))
                    {
                        File.Create(newFile);
                    }

                    using (StreamWriter writer = new StreamWriter(newFile, true))
                    {
                        writer.WriteLine(DateTime.Now + " - EX - " + ex);
                    }
                }



            }
        }


    }
}
