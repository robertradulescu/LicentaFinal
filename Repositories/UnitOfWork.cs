using LicentaFinal.Core.Repositories;
using LicentaFinal.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using LicentaFinal.Data;
namespace LicentaFinal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; }
        public IRoleRepository Role { get; }

        public UnitOfWork(IUserRepository user, IRoleRepository role)
        {
            User = user;
            Role = role;
        }
    }
}
