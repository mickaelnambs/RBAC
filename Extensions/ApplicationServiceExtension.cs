using System;
using Microsoft.EntityFrameworkCore;
using RBAC.Data;

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

        return services;
    }
}
