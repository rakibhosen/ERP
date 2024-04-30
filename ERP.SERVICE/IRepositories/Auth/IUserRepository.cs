using ERP.ENTITY.Models.Auth;
using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.ENTITY.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SERVICE.IRepositories.Auth
{
    public interface IUserRepository
    {
        Task<bool> ValidateUser(string username, string password);
        Task<User> GetUserByUsername(string username);
        Task<bool> CustomUserValidation(string username, string password);
        //Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Company>> GetCompanies();

        Task<IEnumerable<SelectListItem>> GetCompanySelectList();

    }
}
