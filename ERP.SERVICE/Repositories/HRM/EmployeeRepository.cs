using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.SERVICE.IRepositories.HRM;
using ERP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace ERP.SERVICE.Repositories.HRM
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataAccess _dataAccess;

        public EmployeeRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            string comcod = "3101";
            //TransInfoResult result = await _dataAccess.GetTransInfo("3101", "dbo.EntryEmployee", "GetAll");
            //DataSet ds = await _dataAccess.GetTransInfo(comcod, "dbo.EntryEmployee", "GetAll");
            //DataTable dt = ds.Tables[0];
            //List<Employee> students = _Utility.ConvertDataTableToList<Employee>(dt);

            List<Employee> employees = await _dataAccess.GetTransInfo<Employee>(comcod, "dbo.EntryEmployee", "GetAll");

            return employees;
        }


        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            string comcod = "3101";
            //DataSet ds = await _dataAccess.GetTransInfo(comcod, "dbo.EntryEmployee", "GetAll");
            //DataTable dt = ds.Tables[1];
            //List<Employee> students = _Utility.ConvertDataTableToList<Employee>(dt);
            List<Employee> employees = await _dataAccess.GetTransInfo<Employee>(comcod, "dbo.EntryEmployee", "GetAll");
            return employees.FirstOrDefault();
        }


        public async Task<int> AddEmployee(Employee employee)
        {

            var newEmployeeId = (int)_dataAccess.GetScalarValue("AddEmployee",
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Email", employee.Email));

            return newEmployeeId;
        }


        public async Task<bool> UpdateEmployee(Employee employee)
        {

            return await _dataAccess.UpdateTransData("YourCompCode", "dbo.EntryEmployee", "InsertUpdate", employee.EmployeeId.ToString(),employee.FirstName,employee.LastName,employee.EmpImg);
          
        }


        public async Task<bool> DeleteEmployee(int employeeId)
        {
            return await _dataAccess.UpdateTransData("YourCompCode", "DeleteEmployee", "YourCallType","");
        }
    }

}
