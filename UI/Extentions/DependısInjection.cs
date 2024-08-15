using Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_Business.Servicess;

namespace UI.Extentions
{
    public static class DependısInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer("AppConnectonString");
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IMailService, MailService>();
            return services;    

        }
    }
}
