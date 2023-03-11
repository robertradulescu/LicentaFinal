using LicentaFinal.Core.Repositories;
using LicentaFinal.Data;
namespace LicentaFinal.Core.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }
    }
}
