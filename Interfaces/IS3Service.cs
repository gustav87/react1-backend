using Amazon.S3.Model;
using react1_backend.CloudStorage;

namespace react1_backend.S3;

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
