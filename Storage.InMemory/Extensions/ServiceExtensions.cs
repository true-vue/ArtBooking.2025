using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Storage.InMemory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArtBookingStorageInMemory(this IServiceCollection services)
        {
            services.AddDbContext<ArtBookingDbContextInMemory>(options =>
                options.UseInMemoryDatabase("ArtBookingDbInMemory"));
            return services;
        }
    }
}