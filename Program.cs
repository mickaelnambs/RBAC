using Microsoft.EntityFrameworkCore;
using RBAC.Data;
using RBAC.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCorsPolicy();

builder.Services.AddSwaggerServices();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRbacServices();

var app = builder.Build();

app.UseCorsPolicy();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerServices();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    
    await RbacSeedData.SeedRolesAndPermissionsAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();