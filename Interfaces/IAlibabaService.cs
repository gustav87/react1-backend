using System.Net;
using react1_backend.CloudStorage;

namespace react1_backend.Alibaba;

public interface IAlibabaService
{
	List<CloudFile> ListFiles();
	void UploadFile(UploadFileRequest file);
	Task<byte[]> DownloadFile(string fileName);
	HttpStatusCode DeleteFile(string fileName);
}
