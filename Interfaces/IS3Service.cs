using Amazon.S3.Model;

namespace react1_backend.S3;

public interface IS3Service
{
  Task<List<string>> ListFiles();
  void ListFilesInConsole();
	void UploadFile(IFormFile file);
	void UploadFile(UploadFileRequest2 file);
  void UploadFile(string fileName);
	Task<byte[]> DownloadFile(string fileName);
	void DeleteFile(string fileName);
}
