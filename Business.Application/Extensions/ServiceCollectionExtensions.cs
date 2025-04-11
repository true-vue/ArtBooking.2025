using Business.Application.Services.Organizations;
using Business.Application.Services.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Business.Application.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Business.Model.Entities.Users;
using Business.Application.Services.Users;
namespace Business.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArtBookingBusinessLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IArtOrganizationService, ArtOrganizationService>();
            services.AddScoped<IArtEventService, ArtEventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            return services;
        }
    }
}