using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MediSearch.Application
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
              config.RegisterServicesFromAssembly(typeof(ApplicationServices).Assembly)
            );

            services.AddValidatorsFromAssembly(typeof(ApplicationServices).Assembly);

            return services;
        }

    }
}
