using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ParsFile.Application
{
    public static class ApplicationServiceRegistration
    {

        public static IServiceCollection AddApplicatonRegistration(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }

    }
}
