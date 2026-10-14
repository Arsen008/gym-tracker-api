using System.IO;

namespace GymTracker.Application.Models;

public record FileData(Stream Stream, string BucketName, string ObjectName)
{
    
}