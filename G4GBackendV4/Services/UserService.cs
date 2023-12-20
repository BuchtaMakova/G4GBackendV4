using G4GBackendV4.Data;
using G4GBackendV4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G4GBackendV4.Services
{
    public class UserService : ModelServiceBase
    {
        private readonly G4GDbContext _context;
        private readonly SecurityService _securityService;

        public UserService([FromServices] G4GDbContext context, [FromServices] SecurityService securityService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _securityService = securityService;
        }

        public async Task<User> GetUserByCredentials(string? username, string? password)
        {
            var user = await _context.Users!.Include(q => q.Roles)
                           .FirstOrDefaultAsync(q => q.Username == username.ToLower())
                       ?? throw CreateException($"User {username} does not exist.");
            if (!_securityService.VerifyPassword(password, user.PasswordHash!))
                throw CreateException("Credentials are not valid.");

            return user;
        }

        public async Task<User> Create(string username, string password)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(password, nameof(password));

            username = username.ToLower();

            if (_context.Users!.Any(q => q.Username == username))
                throw CreateException($"User {username} already exists.");
            var hash = _securityService.HashPassword(password);
            var roles = await (_context.Roles ??
                               throw new InvalidOperationException($"role {CustomRoles.User} does not exists"))
                .Where(q => q.Name == CustomRoles.User).ToListAsync();
            var ret = new User { Username = username, PasswordHash = hash, Roles = roles };

            _context.Users!.Add(ret);
            await _context.SaveChangesAsync();

            return ret;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users!.Where(q => q.Username == username).Include(ac => ac.Contents)
                       .Include(q => q.Roles)
                       .FirstAsync() ??
                   throw CreateException($"User with id {username} does not exist");
        }
    }
}