using System.Net;
using React1_backend.CloudStorage;

namespace React1_backend.Alibaba;

public interface IAlibabaService
{
	List<CloudFile> ListFiles();
	void UploadFile(UploadFileRequest file);
	Task<byte[]> DownloadFile(string fileName);
	HttpStatusCode DeleteFile(string fileName);
}
