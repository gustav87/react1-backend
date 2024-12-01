using System.Net;
using React1_Backend.Contracts;

namespace React1_Backend.Alibaba;

public interface IAlibabaService
{
    List<CloudFile> ListFiles();
    void UploadFile(UploadFileRequest file);
    Task<byte[]> DownloadFile(string fileName);
    HttpStatusCode DeleteFile(string fileName);
}
