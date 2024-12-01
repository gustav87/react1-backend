using React1_Backend.Contracts;

namespace React1_Backend.S3;

public interface IS3Service
{
    Task<List<CloudFile>> ListFiles();
    void ListFilesInConsole();
    void UploadFile(IFormFile file);
    void UploadFile(UploadFileRequest file);
    void UploadFile(string fileName);
    Task<byte[]> DownloadFile(string fileName);
    void DeleteFile(string fileName);
}
