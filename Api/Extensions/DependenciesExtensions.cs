using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Domain.Interfaces;
using Persistence.Data;

namespace Api.Extensions
{
    public static class DependenciesExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IHandler<,>), typeof(Handler<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
