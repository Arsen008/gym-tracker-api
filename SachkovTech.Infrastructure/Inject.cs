using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;
using SachkovTech.Application.Exercises;
using SachkovTech.Application.Workouts;
using SachkovTech.Infrastructure.Interceptors;
using SachkovTech.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Minio;
using SachkovTech.Application.Providers;
using SachkovTech.Infrastructure.Options;
using SachkovTech.Infrastructure.Providers;
using MinioOptions = SachkovTech.Infrastructure.Options.MinioOptions;

namespace SachkovTech.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWorkoutsRepository, WorkoutsRepository>();
        services.AddScoped<IExercisesRepository, ExercisesRepository>();
        services.AddScoped<IExerciseTypesRepository, ExerciseTypesRepository>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<IFileProvider, MinioProvider>();
        services.AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.Minio));

        var minioOptions = configuration.GetSection(MinioOptions.Minio).Get<MinioOptions>()
                           ?? throw new ApplicationException("Missing minio configuration");

        services.AddMinio(options =>
        {
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        return services;
    }
}