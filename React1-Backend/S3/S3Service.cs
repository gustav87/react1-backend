using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using React1_Backend.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace React1_Backend.S3;

public class S3Service : IS3Service
{
    private readonly string bucketName = "gustavbucket1";
    private readonly string AWS_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? "";
    private readonly string AWS_SECRET_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_KEY") ?? "";
    private readonly Amazon.RegionEndpoint awsEndpoint = Amazon.RegionEndpoint.EUNorth1;
    // private readonly string awsPrefix = "";

    public async Task<List<CloudFile>> ListFiles()
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        try
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = ""
            };
            ListObjectsResponse listResponse = await client.ListObjectsAsync(listRequest);
            var files = listResponse.S3Objects
              .Where(x => x.Size != 0) // Filter out the directory containing the files
              .Select(x => new CloudFile
              {
                  Name = x.Key.Substring(x.Key.LastIndexOf("/") + 1),
                  Size = x.Size / 1024, // Size in kilobytes
                  LastModified = x.LastModified
              }).ToList();
            return files;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async void ListFilesInConsole()
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        try
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = ""
            };
            ListObjectsResponse listResponse;
            do
            {
                listResponse = await client.ListObjectsAsync(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    Console.WriteLine(obj.Key);
                    Console.WriteLine(" Size - " + obj.Size);
                }

                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async void UploadFile(string filePath)
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        try
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                ContentType = "text/plain"
            };

            PutObjectResponse response = await client.PutObjectAsync(putObjectRequest);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async void UploadFile(IFormFile file)
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        try
        {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = file.FileName,
                BucketName = bucketName,
                ContentType = file.ContentType
            };

            var fileTransferUtility = new TransferUtility(client);

            await fileTransferUtility.UploadAsync(uploadRequest);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async void UploadFile(UploadFileRequest file)
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);
        try
        {
            byte[] bytes = Convert.FromBase64String(file.Content);
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = file.Name
            };
            using var ms = new MemoryStream(bytes);
            putObjectRequest.InputStream = ms;
            PutObjectResponse response = await client.PutObjectAsync(putObjectRequest);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<byte[]> DownloadFile(string fileName)
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        MemoryStream ms = null;

        try
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            using (var response = await client.GetObjectAsync(getObjectRequest))
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

    public async void DeleteFile(string fileName)
    {
        var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, awsEndpoint);

        DeleteObjectRequest request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        await client.DeleteObjectAsync(request);
    }
}
