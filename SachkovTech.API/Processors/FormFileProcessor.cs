using SachkovTech.Application.Exercises.UploadExerciseMedias;

namespace SachkovTech.API.Extensions.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<UploadFileDto> _fileDtos = [];

    public List<UploadFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            
            var fileDto = new UploadFileDto(stream , file.FileName);
            _fileDtos.Add(fileDto);
        }
        return _fileDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var dto in _fileDtos)
          await  dto.Stream.DisposeAsync();
    }
}