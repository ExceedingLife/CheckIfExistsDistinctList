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
        public string Keyword { get; set; }

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
            Keyword = _config.GetSection("Parameters:Keyword").Value;
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
                    string[] lstFileLines = File.ReadAllLines(filePath);

                    if (lstFileLines.Any())
                    {
                        if (CreateNewFile())
                        {
                            #region v1 Create New File
                            //if (!File.Exists(newFile))
                            //{
                            //    File.Create(newFile);
                            //} 
                            #endregion
                                                       
                            using (StreamWriter writer = new StreamWriter(newFile, true))
                            {
                                foreach (var line in lstFileLines.Distinct())
                                {
                                    await writer.WriteLineAsync(line);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (CreateNewFile())
                    {
                        #region v1 Create New File
                        //if (!File.Exists(newFile))
                        //{
                        //    File.Create(newFile);
                        //} 
                        #endregion

                        using (StreamWriter writer = new StreamWriter(newFile, true))
                        {
                            writer.WriteLine(DateTime.Now + " - EX - " + ex);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if (CreateNewFile())
                    {
                        #region v1 Create New File
                        //if (!File.Exists(newFile))
                        //{
                        //    File.Create(newFile);
                        //} 
                        #endregion

                        using (StreamWriter writer = new StreamWriter(newFile, true))
                        {
                            writer.WriteLine(DateTime.Now + " - File Reading Error, Does NOT Exist");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (CreateNewFile())
                    {
                        #region v1 Create New File
                        //if (!File.Exists(newFile))
                        //{
                        //    File.Create(newFile);
                        //} 
                        #endregion

                        using (StreamWriter writer = new StreamWriter(newFile, true))
                        {
                            writer.WriteLine(DateTime.Now + " - EX - " + ex);
                        }
                    }
                }

            }
        }


        public async Task RunFileCheckKeywordAndCreate()
        {
            string filePath = Path.Combine(FolderPath, FileName);
            string newFile = Path.Combine(FolderPath, NewFileName);

            if (File.Exists(filePath))
            {
                try
                {
                    string[] lstFileLines = File.ReadAllLines(filePath);
                    if ((lstFileLines.Any()) && !(string.IsNullOrWhiteSpace(Keyword)))
                    {
                        if (CreateNewFile())
                        {
                            List<string> lstDistinct = new List<string>();
                            foreach (var line in lstFileLines)
                            {
                                var startIndex = line.IndexOfAny("_logger".ToCharArray());
                                var lastIndex = line.LastIndexOfAny("_logger".ToCharArray());

                                Console.WriteLine(startIndex);
                                Console.WriteLine(lastIndex);
                                Console.WriteLine(line);
                                Console.WriteLine();

                                var keywordLastIndex = line.LastIndexOfAny(Keyword.ToCharArray());
                                Console.WriteLine(keywordLastIndex);
                                var stringIP = line.Substring(keywordLastIndex);
                                lstDistinct.Add(stringIP);
                            }

                            using (StreamWriter writer = new StreamWriter(newFile, true))
                            {
                                foreach (var line in lstDistinct.Distinct())
                                {
                                    await writer.WriteLineAsync(line);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (CreateNewFile())
                    {
                        using (StreamWriter writer = new StreamWriter(newFile, true))
                        {
                            writer.WriteLine(DateTime.Now + " - EX - " + ex);
                        }
                    }
                }
            }

        }


        public bool CreateNewFile()
        {
            if (!(string.IsNullOrWhiteSpace(FolderPath) && !(string.IsNullOrWhiteSpace(NewFileName))))
            {
                try
                {
                    string newFile = Path.Combine(FolderPath, NewFileName);

                    if (!File.Exists(newFile))
                    {
                        File.Create(newFile);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}
