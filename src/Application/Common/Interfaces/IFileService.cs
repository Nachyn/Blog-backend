using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        int GetCountCatalogs(string path);

        Task WriteToStorageAsync(IFormFile uploadedFile, string filePath);

        void DeleteFilesFromStorage(string pathFromRoot,
            params string[] filePaths);

        FileContentResult GetFileFromStorage(string filePathFromRoot,
            string fileDownloadName);
    }
}