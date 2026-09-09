using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using SachkovTech.Application.Models;
using SachkovTech.Application.Providers;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(fileData.BucketName), cancellationToken);

            if (!bucketExist)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(fileData.BucketName), cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.ObjectName);

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return fileData.ObjectName; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке файла {ObjectName}", fileData.ObjectName);
            return Error.Failure("storage.upload_failed", "Ошибка при загрузке файла.");
        }
    }

    public async Task<Result<string, Error>> RemoveFile(string bucketName, string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении файла {ObjectName}", objectName);
            return Error.Failure("storage.delete_failed", "Ошибка при удалении файла.");
        }
    }

    public async Task<Result<string, Error>> GetPresignedUrl(string bucketName, string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var presignedArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(60 * 60 * 24);  

            var url = await _minioClient.PresignedGetObjectAsync(presignedArgs);
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении ссылки на файл {ObjectName}", objectName);
            return Error.Failure("storage.link_failed", "Ошибка при генерации ссылки.");
        }
    }
}