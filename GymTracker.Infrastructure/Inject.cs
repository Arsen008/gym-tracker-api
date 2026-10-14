using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;
using GymTracker.Application.Exercises;
using GymTracker.Application.Workouts;
using GymTracker.Infrastructure.Interceptors;
using GymTracker.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Minio;
using GymTracker.Application.Providers;
using GymTracker.Infrastructure.Options;
using GymTracker.Infrastructure.Providers;
using MinioOptions = GymTracker.Infrastructure.Options.MinioOptions;

namespace GymTracker.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWorkoutsRepository, WorkoutsRepository>();
        services.AddScoped<IExercisesRepository, ExercisesRepository>();
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