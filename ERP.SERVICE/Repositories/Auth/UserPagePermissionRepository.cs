using System.Data;
using System.Threading.Tasks;

namespace ERP.SERVICE.IRepositories.Auth
{
    public class UserPagePermissionRepository : IUserPagePermissionRepository
    {
        private readonly DataAccess _dataAccess;

        public UserPagePermissionRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<bool> HasPermission(string userId, string pageId)
        {

            //DataSet ds = await _dataAccess.GetTransInfo("comcod", "Permission", "PagePermission", userId, pageId);

            // Check if the user has permission based on the result from the database
            //DataTable dt = ds.Tables[0];
            //bool hasPermission = dt.Rows.Count > 0; 

            //return hasPermission;
            return true;
        }

    }
}
