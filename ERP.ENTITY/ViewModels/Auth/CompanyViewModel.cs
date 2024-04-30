using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.ViewModels.Auth
{
    public class CompanyViewModel
    {
        public string SelectedComcod { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
