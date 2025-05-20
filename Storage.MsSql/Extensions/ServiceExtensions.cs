using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Business.Application.Repositories;
using Storage.MsSql.Repositories;

namespace Storage.MsSql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArtBookingStorageMsSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ArtBookingDbContextMsSql>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IArtEventRepository, ArtEventRepository>();

            return services;
        }
    }
}