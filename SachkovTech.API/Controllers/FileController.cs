using Microsoft.AspNetCore.Mvc;
using Minio;

namespace SachkovTech.API.Controllers;

public class FileController : ApplicationController
{
    private readonly IMinioClient _minioClient;

    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFile()
    {
        var buckets = await _minioClient.ListBucketsAsync();
        var bucketsString = string.Join(", ", buckets.Buckets.Select(b => b.Name));
        return Ok(bucketsString);
    }
}