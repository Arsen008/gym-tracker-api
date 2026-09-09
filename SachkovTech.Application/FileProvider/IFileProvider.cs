using CSharpFunctionalExtensions;
using SachkovTech.Application.Models;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> RemoveFile(string bucketName, string objectName,
        CancellationToken cancellationToken = default);
    Task<Result<string, Error>> GetPresignedUrl(string bucketName, string objectName,
        CancellationToken cancellationToken = default);
}