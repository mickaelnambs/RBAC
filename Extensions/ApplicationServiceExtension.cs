using Microsoft.EntityFrameworkCore;
using RBAC.Data;
using RBAC.Services.Implementations;
using RBAC.Services.Interfaces;

namespace RBAC.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt => 
        {
            opt.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                });
        });

        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }
}
