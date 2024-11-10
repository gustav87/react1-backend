using System.Net;
using Aliyun.OSS;
using React1_backend.Contracts;

namespace React1_backend.Alibaba;

public class AlibabaService : IAlibabaService
{
    private readonly string bucketName = "gstavbucket1";
    private readonly string alibabaAccessKey = Environment.GetEnvironmentVariable("alibabaAccessKey") ?? "";
    private readonly string alibabaSecretKey = Environment.GetEnvironmentVariable("alibabaSecretKey") ?? "";
    private readonly string alibabaEndpoint = "https://oss-eu-central-1.aliyuncs.com";
    private readonly string alibabaPrefix = "test/";

    public List<CloudFile> ListFiles()
    {
        var client = new OssClient(alibabaEndpoint, alibabaAccessKey, alibabaSecretKey);
        var listObjectsRequest = new ListObjectsRequest(bucketName)
        {
            MaxKeys = 200, // The default value of MaxKeys is 100. The maximum value is 1000.
            Prefix = alibabaPrefix
        };
        var result = client.ListObjects(listObjectsRequest);
        Console.WriteLine("List objects succeeded");
        foreach (var summary in result.ObjectSummaries)
        {
            Console.WriteLine("File name:{0}", summary.Key);
        }
        return result.ObjectSummaries
          .Where(x => x.Size != 0) // Filter out the directory containing the files
          .Select(x => new CloudFile
          {
              Name = x.Key.Substring(x.Key.LastIndexOf("/") + 1),
              Size = x.Size / 1024, // Size in kilobytes
              LastModified = x.LastModified
          }).ToList();
    }

    public void UploadFile(UploadFileRequest file)
    {
        var client = new OssClient(alibabaEndpoint, alibabaAccessKey, alibabaSecretKey);
        byte[] bytes = Convert.FromBase64String(file.Content);
        using var ms = new MemoryStream(bytes);
        var putObjectRequest = new PutObjectRequest(bucketName, $"{alibabaPrefix}{file.Name}", ms);
        PutObjectResult response = client.PutObject(putObjectRequest);
    }

    public async Task<byte[]> DownloadFile(string fileName)
    {
        var client = new OssClient(alibabaEndpoint, alibabaAccessKey, alibabaSecretKey);

        MemoryStream? ms = null;

        try
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, $"{alibabaPrefix}{fileName}");

            using (var response = client.GetObject(getObjectRequest))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException(string.Format("The file '{0}' is not found", fileName));

            return ms.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public HttpStatusCode DeleteFile(string fileName)
    {
        var client = new OssClient(alibabaEndpoint, alibabaAccessKey, alibabaSecretKey);
        DeleteObjectResult deleteObjectResult = client.DeleteObject(bucketName, $"{alibabaPrefix}{fileName}");
        return deleteObjectResult.HttpStatusCode;
    }
}
