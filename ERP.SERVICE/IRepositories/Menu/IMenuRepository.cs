using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.ENTITY.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SERVICE.IRepositories.Menu
{
    public interface IMenuRepository
    {
        Task<IEnumerable<ModuleInfo>> GetModules();
        Task<IEnumerable<MenuInfo>> GetMenus(int moduleid,string userid);

    }
}
