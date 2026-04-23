using Citrus_Backend.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Citrus_Backend.Alibaba;

public interface IAlibabaService
{
    List<CloudFile> ListFiles();
    void UploadFile(UploadFileRequest file);
    Task<byte[]> DownloadFile(string fileName);
    HttpStatusCode DeleteFile(string fileName);
}
