using LicentaFinal.Areas.Identity.Data;
using LicentaFinal.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using LicentaFinal.Data;

namespace LicentaFinal.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
