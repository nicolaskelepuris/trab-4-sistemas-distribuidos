using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Domain.Interfaces;
using Persistence.Data;
using System.Reflection;
using System.Linq;

namespace Api.Extensions
{
    public static class DependenciesExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ICpfValidator, CpfValidator>();
            services.AddScoped<IBlockchain, BlockchainSimulator>();
            
            Assembly
                .GetAssembly(typeof(Application.Handlers.OrganRequests.CreateOrganRequestHandler))
                ?.GetTypes()
                .Where(a => a.Name.EndsWith("Handler") && !a.IsAbstract && !a.IsInterface)
                .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
                .ToList()
                .ForEach(typesToRegister =>
                {
                    typesToRegister.serviceTypes.ForEach(typeToRegister => services.AddScoped(typeToRegister, typesToRegister.assignedType));
                });
        }
    }
}
