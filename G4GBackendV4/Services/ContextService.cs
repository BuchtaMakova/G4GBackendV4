using G4GBackendV4.Data;

namespace G4GBackendV4.Services
{
    public class ContextService : ModelServiceBase
    {
        private readonly G4GDbContext _context;

        public ContextService(G4GDbContext context)
        {
            _context = context;
        }

        public G4GDbContext GetContext()
        {
            return _context;
        }
    }
}
