using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Commands.Storage;
using Contract;
using Conversions.FileConversions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System;
using System.Net;
using System.Security.AccessControl;
using XAct.Messages;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Core.Services.Storage
{
    public class FileStorgaeCommunicationService : IFileStorgaeCommunicationService
    {
        private const int TIMEOUT = 2500;

        // Digital Ocean settings
        private static readonly string S3LoginRoot = "https://dot-net-app-space.fra1.digitaloceanspaces.com";
        private static readonly string S3BucketName = "telemedicine";
        private static readonly string AccessKey = "DO00Z93EZCGJ92WMGWX6";
        private static readonly string AccessKeySecret = "axaGigJhp9EY8dIicIoYDSgMouX2FEI7qvXEHt1bggs";
        private static readonly string S3FolderName = "test";
        private readonly ILogger<FileStorgaeCommunicationService> _logger;

        public FileStorgaeCommunicationService(ILogger<FileStorgaeCommunicationService> logger)
        {
            _logger = logger;
        }

        public bool UploadFile(FileUploadCommand command)
        {
            try
            {
                AmazonS3Client? s3Client = new(new BasicAWSCredentials(AccessKey, AccessKeySecret), new AmazonS3Config
                {
                    ServiceURL = S3LoginRoot,
                    Timeout = TimeSpan.FromSeconds(TIMEOUT),
                    MaxErrorRetry = 8,
                });

                byte[] bytes = System.Convert.FromBase64String(command?.Base64 ?? "");

                TransferUtility fileTransferUtility = new(s3Client);
                
                var fileExtension = Path.GetExtension(command?.FileName);

                var contentType = FileMappings.GetContentType(fileExtension);

                TransferUtilityUploadRequest? fileTransferUtilityRequest = new()
                {
                    BucketName = S3BucketName + @"/" + S3FolderName,
                    Key = command?.FileId,
                    InputStream = new MemoryStream(bytes),
                    ContentType = contentType,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456,
                    CannedACL = S3CannedACL.PublicRead
                };

                fileTransferUtility.Upload(fileTransferUtilityRequest);

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in FileStorgaeCommunicationService class, Upload methid \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return false;
            }
        }

        public async Task<string> GetFileAsBase64(string fileId)
        {
            try
            {
                AmazonS3Client? s3Client = new(new BasicAWSCredentials(AccessKey, AccessKeySecret), new AmazonS3Config
                {
                    ServiceURL = S3LoginRoot,
                    Timeout = TimeSpan.FromSeconds(TIMEOUT),
                    MaxErrorRetry = 15,
                });

                var response = await s3Client.GetObjectAsync(new GetObjectRequest { BucketName = S3BucketName + @"/" + S3FolderName, Key = fileId });
                MemoryStream memoryStream = new MemoryStream();

                using (Stream responseStream = response.ResponseStream)
                {
                    responseStream.CopyTo(memoryStream);
                }

                byte[] bytes = memoryStream.ToArray();

                return Convert.ToBase64String(bytes);
               
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in FileStorgaeCommunicationService class, Upload methid \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);
            }

            return null;
        }
        
        public async Task DeleteFileAsync(string fileId)
        {
           
            AmazonS3Client? s3Client = new(new BasicAWSCredentials(AccessKey, AccessKeySecret), new AmazonS3Config
            {
                ServiceURL = S3LoginRoot,
                Timeout = TimeSpan.FromSeconds(TIMEOUT),
                MaxErrorRetry = 15,
            });

            await s3Client.DeleteObjectAsync(new Amazon.S3.Model.DeleteObjectRequest() { BucketName = S3BucketName + @"/" + S3FolderName, Key = fileId });
        }
    }
}
