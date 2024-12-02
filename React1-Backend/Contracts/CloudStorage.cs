using System;

namespace React1_Backend.Contracts;

public class CloudFile
{
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}

public class UploadFileRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
}
