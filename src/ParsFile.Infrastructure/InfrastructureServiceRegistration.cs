using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParsFile.Application.Contracts.Initializer;
using ParsFile.Application.Contracts.Repositories.Content;
using ParsFile.Application.Contracts.Repositories.Identity;
using ParsFile.Application.Contracts.Services;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Domain.Models;
using ParsFile.Infrastructure.Helpers;
using ParsFile.Infrastructure.Persistence.Data;
using ParsFile.Infrastructure.Persistence.Initializer;
using ParsFile.Infrastructure.Repositories.Content;
using ParsFile.Infrastructure.Repositories.Identity;
using ParsFile.Infrastructure.Services.Mail;

namespace ParsFile.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastuctureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // sql server registeration
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));

            // resgiter asp-identity 
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole>()
                    .AddPasswordValidator<PasswordValidator>()
                    .AddErrorDescriber<PersianIdentityError>();

            // db initializer
            services.AddScoped<IDbInitializer, DbInitializer>();

            // session registration
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            services.AddServiceDependencies(configuration);
            services.AddRepositoryDependencies();

            return services;
        }

        private static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // configure email settings
            var emailSettings = configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(emailSettings);


            services.AddSingleton<IEmailService, EmailService>();
            return services;
        }

        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
            services.AddScoped<ICategoryRepsitory, CategoryRepostory>();
            return services;
        }
    }
}
