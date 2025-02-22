using Microsoft.EntityFrameworkCore;
using RBAC.Entities;

namespace RBAC.Data;

public static class RbacSeedData
{
    public static async Task SeedRolesAndPermissionsAsync(DataContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Admin", Description = "Full access to all features" },
                new Role { Name = "Manager", Description = "Access to manage content and users" },
                new Role { Name = "Editor", Description = "Access to create and edit content" },
                new Role { Name = "Customer", Description = "Regular user with basic access" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        if (!await context.Permissions.AnyAsync())
        {
            var permissions = new List<Permission>
            {
                new Permission { Name = "users.view", Description = "View user details", Module = "Users" },
                new Permission { Name = "users.create", Description = "Create new users", Module = "Users" },
                new Permission { Name = "users.edit", Description = "Edit user details", Module = "Users" },
                new Permission { Name = "users.delete", Description = "Delete users", Module = "Users" },

                new Permission { Name = "products.view", Description = "View products", Module = "Products" },
                new Permission { Name = "products.create", Description = "Create products", Module = "Products" },
                new Permission { Name = "products.edit", Description = "Edit products", Module = "Products" },
                new Permission { Name = "products.delete", Description = "Delete products", Module = "Products" },

                new Permission { Name = "orders.view", Description = "View orders", Module = "Orders" },
                new Permission { Name = "orders.create", Description = "Create orders", Module = "Orders" },
                new Permission { Name = "orders.edit", Description = "Edit orders", Module = "Orders" },
                new Permission { Name = "orders.delete", Description = "Delete orders", Module = "Orders" },

                new Permission { Name = "reviews.view", Description = "View reviews", Module = "Reviews" },
                new Permission { Name = "reviews.create", Description = "Create reviews", Module = "Reviews" },
                new Permission { Name = "reviews.edit", Description = "Edit reviews", Module = "Reviews" },
                new Permission { Name = "reviews.delete", Description = "Delete reviews", Module = "Reviews" }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        await AssignPermissionsToRoles(context);
        await CreateAdminUserAsync(context);
    }

    private static async Task AssignPermissionsToRoles(DataContext context)
    {
        var roles = await context.Roles.ToListAsync();
        var permissions = await context.Permissions.ToListAsync();
        
        Role GetRole(string name) => roles.First(r => r.Name == name);
        
        IEnumerable<Permission> GetPermissionsByModule(string module) => 
            permissions.Where(p => p.Module == module);

        var adminRole = GetRole("Admin");
        foreach (var permission in permissions)
        {
            if (!await context.RolePermissions.AnyAsync(rp => 
                rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    Role = adminRole,
                    Permission = permission
                });
            }
        }

        var managerRole = GetRole("Manager");
        foreach (var permission in permissions.Where(p => 
            p.Name.EndsWith(".view") || p.Name.EndsWith(".edit") || p.Name.EndsWith(".create")))
        {
            if (!await context.RolePermissions.AnyAsync(rp => 
                rp.RoleId == managerRole.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    Role = managerRole,
                    Permission = permission
                });
            }
        }

        var editorRole = GetRole("Editor");
        foreach (var permission in GetPermissionsByModule("Products")
            .Concat(GetPermissionsByModule("Reviews"))
            .Where(p => !p.Name.EndsWith(".delete")))
        {
            if (!await context.RolePermissions.AnyAsync(rp => 
                rp.RoleId == editorRole.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    Role = editorRole,
                    Permission = permission
                });
            }
        }

        var customerRole = GetRole("Customer");
        foreach (var permission in permissions.Where(p => 
            p.Name.EndsWith(".view") || p.Name == "reviews.create" || p.Name == "orders.create"))
        {
            if (!await context.RolePermissions.AnyAsync(rp => 
                rp.RoleId == customerRole.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    Role = customerRole,
                    Permission = permission
                });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task CreateAdminUserAsync(DataContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Email == "admin@example.com"))
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Pa$$w0rd");
            
            var adminUser = new User
            {
                Name = "Admin User",
                Email = "admin@example.com",
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            await context.UserRoles.AddAsync(new UserRole
            {
                User = adminUser,
                Role = adminRole
            });
            
            await context.SaveChangesAsync();
        }
    }
}
