using System.Threading.Tasks;

namespace ERP.SERVICE.IRepositories.Auth
{
    public interface IUserPagePermissionRepository
    {
        Task<bool> HasPermission(string userId, string pageId);
    }
}
