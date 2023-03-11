using LicentaFinal.Areas.Identity.Data;
using LicentaFinal.Data;

namespace LicentaFinal.Core.Repositories
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();

        ApplicationUser GetUser(string id);

        ApplicationUser UpdateUser(ApplicationUser user);
    }
}
