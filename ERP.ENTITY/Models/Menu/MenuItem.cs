using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.Models.Menu
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
        public int ModuleId { get; set; }  // Add ModuleId property
        public Dictionary<string, List<string>> CompanyPermissions { get; set; }
        public List<MenuItem> SubMenuItems { get; set; }
    }

}
