using G4GBackendV4.Data;
using G4GBackendV4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4GBackendV4.Services
{
    public class RoleService
    {
        private readonly G4GDbContext _context;

        public RoleService([FromServices] G4GDbContext context)
        {
            _context = context;
        }

        public async Task<Role> CreateRole(string name)
        {
            if ((_context.Roles?.Where(q => q.Name == name) ?? throw new InvalidOperationException("roles null")).Any())
            {
                throw new InvalidOperationException("Role already exists");
            }

            var role = new Role
            {
                Name = name
            };
            _context.Roles?.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task AddRoleToUser(string username, string roleName)
        {
            var user = await _context.Users?.FirstOrDefaultAsync(q => q.Username == username);
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist");
            }

            var role = await _context.Roles?.FirstOrDefaultAsync(q => q.Name == roleName);
            if (role == null)
            {
                throw new InvalidOperationException("Role does not exist");
            }

            user.Roles.Add(role);
            await _context.SaveChangesAsync();
        }
    }
}