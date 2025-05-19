using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Storage.MsSql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArtBookingStorageInMemory(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ArtBookingDbContextMsSql>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}