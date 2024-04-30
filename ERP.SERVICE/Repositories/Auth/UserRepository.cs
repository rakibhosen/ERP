using ERP.ENTITY.Models.Auth;
using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.ENTITY.ViewModels.Auth;
using ERP.SERVICE.IRepositories.Auth;
using ERP.UTILITY;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace ERP.SERVICE.Repositories.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly DataAccess _dataAccess;

        public UserRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public async Task<bool> ValidateUser(string username, string password)
        {
            //DataSet ds = await _dataAccess.GetTransInfo("3101", "YourCustomProcedure", "YourCustomCallType");

            //DataTable dt = ds.Tables[0];
            //var user = _Utility.ConvertDataTableToList<User>(dt).FirstOrDefault();
            var  user = await _dataAccess.GetTransInfo<ValidateUser>("3101", "SP_ENTRY_USER_AUTH", "ValidateUser", username, password);
            return user.Count() > 0 ? true : false;

        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _dataAccess.GetTransInfo<User>("3101", "SP_ENTRY_USER_AUTH", "GetUserByUsername", username);
            return user.FirstOrDefault() ?? new User();

        }

        public async Task<bool> CustomUserValidation(string username, string password)
        {
            List<User> user = await _dataAccess.GetTransInfo<User>("3101", "SP_ENTRY_USER_AUTH", "ValidateUser", username, password);
            return user.Count() > 0 ? true : false;
        }
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            //DataSet ds = await _dataAccess.GetTransInfo("3101", "dbo.EntryEmployee", "GetCompInfo");
            //DataTable dt = ds.Tables[0];
            //var companies = _Utility.ConvertDataTableToList<Company>(dt);
            List<Company> companies = await _dataAccess.GetTransInfo<Company>("3101", "dbo.EntryEmployee", "GetCompInfo");
            return companies;

        }
        public async Task<IEnumerable<SelectListItem>> GetCompanySelectList()
        {
            var comlist = await GetCompanies(); // Implement this method to get companies
            return comlist.Select(c => new SelectListItem { Value = c.comcod, Text = c.comnam });
        }


    }
}
