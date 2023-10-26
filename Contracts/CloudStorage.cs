using System;

namespace react1_backend.CloudStorage;

public class CloudFile
{
  public string Name { get; set; }
  public long Size { get; set; }
  public DateTime LastModified {get; set; }
}

public class UploadFileRequest
{
  public string Name { get; set; }
  public string Content { get; set; }
}
