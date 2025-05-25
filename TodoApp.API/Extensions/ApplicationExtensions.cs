using Microsoft.EntityFrameworkCore;
using TodoApp.Data.DbContexts;
using TodoApp.Data.Services.Implementations.Sql;
using TodoApp.Data.Services.Interfaces;

namespace TodoApp.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<AppDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlServer(configuration["ConnectionStrings:TodoAppDb"]));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ITodoService, TodoService>();

            return services;
        }
    }
}
