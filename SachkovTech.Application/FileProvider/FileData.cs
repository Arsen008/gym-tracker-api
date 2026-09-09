using System.IO;

namespace SachkovTech.Application.Models;

public record FileData(Stream Stream, string BucketName, string ObjectName);