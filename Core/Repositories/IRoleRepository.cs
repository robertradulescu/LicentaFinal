using LicentaFinal.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using LicentaFinal.Data;
namespace LicentaFinal.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
