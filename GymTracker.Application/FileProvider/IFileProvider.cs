using CSharpFunctionalExtensions;
using GymTracker.Application.Models;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> RemoveFile(string bucketName, string objectName,
        CancellationToken cancellationToken = default);
    Task<Result<string, Error>> GetPresignedUrl(string bucketName, string objectName,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<string>, Error>> UploadFiles(IEnumerable<FileData> files,
        CancellationToken cancellationToken = default);

}