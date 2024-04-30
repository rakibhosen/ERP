using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.ViewModels.Auth
{
    public class LoginViewModel
    {

            public string SelectedCompanyCode { get; set; }
            public IEnumerable<SelectListItem> CompanyList { get; set; }

            public string Username { get; set; }
            public string Password { get; set; }
        

    }
}
