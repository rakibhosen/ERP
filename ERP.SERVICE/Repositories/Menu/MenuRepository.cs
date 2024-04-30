using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.ENTITY.Models.Menu;
using ERP.SERVICE.IRepositories.HRM;
using ERP.SERVICE.IRepositories.Menu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace ERP.SERVICE.Repositories.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DataAccess _dataAccess;

        public MenuRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<ModuleInfo>> GetModules()
        {

            var allModules = await _dataAccess.GetTransInfo<ModuleInfoDTO>("3101", "SP_UTILITY_PAGEPERMISSION", "GetAllModules");

            var permissionLookup = new Dictionary<int, List<ModulePermission>>();

            foreach (var row in allModules)
            {
                int moduleId = row.ModuleId;


                if (!permissionLookup.ContainsKey(moduleId))
                {
                    permissionLookup[moduleId] = new List<ModulePermission>();
                }

                // Add ModulePermission object to the list
                permissionLookup[moduleId].Add(new ModulePermission
                {
                    ModuleId = moduleId,
                    submodname = row.submodname,
                    controller=row.controller,
                    action=row.action,
                    area=row.area,
                    icon=row.icon,
                    qtype=row.qtype,
                    mid=row.mid

                });
            }


            var moduleInfos = new List<ModuleInfo>();

            foreach (var row in allModules.GroupBy(r => r.ModuleId)) // Group by ModuleId
            {
                int moduleId = row.Key;


                var moduleInfo = new ModuleInfo
                {
                    ModuleId = moduleId,
                    name = row.First().name,
      
                    modicon = row.First().modicon,
                    ModulePermissions = permissionLookup.ContainsKey(moduleId) ? permissionLookup[moduleId] : new List<ModulePermission>()
                };

                // Add ModuleInfo to the list
                moduleInfos.Add(moduleInfo);
            }

            return moduleInfos;



            
        }

        public async Task<IEnumerable<MenuInfo>> GetMenus(int menuid,string userid)
        {

            var allMenus = await _dataAccess.GetTransInfo<MenuInfo>("3101", "SP_UTILITY_PAGEPERMISSION", "GetAllMenus", menuid.ToString(),userid);


            return allMenus;


        }
    }
}

