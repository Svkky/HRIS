using AutoMapper;
using FluentValidation;
using HRIS.Application.Behaviours;
using HRIS.Application.DapperServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRIS.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IDapper, Dapperr>();

        }
    }
}
