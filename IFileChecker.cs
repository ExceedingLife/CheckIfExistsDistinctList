using System.Threading.Tasks;

namespace CheckIfExistsDistinctList
{
    public interface IFileChecker
    {
        string FileName { get; set; }
        string FolderPath { get; set; }
        string NewFileName { get; set; }

        Task RunFileCheckAndCreate();
        Task RunFileCheckKeywordAndCreate();
    }
}