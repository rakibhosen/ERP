using ERP.ENTITY.Models.HRM._01.CodeBook;
using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.ENTITY.Models.Menu;
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
    public class CodeBookRepository : ICodeBookReposiroty
    {
        private readonly DataAccess _dataAccess;

        public CodeBookRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<codebook>> GetCode()
        {
            string comcod = "3101";
            var codebook = await _dataAccess.GetTransInfo<codebook>(comcod, "dbo.EntryEmployee", "GETCODE");
            return codebook;
        }


        public async Task<codebook> GetCodeById(string code)
        {
            string comcod = "3101";

            List<codebook> employees = await _dataAccess.GetTransInfo<codebook>(comcod, "dbo.EntryEmployee", "GetAll");
            return employees.FirstOrDefault();
        }


        public async Task<string> AddCode(codebook code)
        {
            string comcod = "3101";
            try
            {
                var codebooks = await _dataAccess.GetTransInfo<codebook>(comcod, "dbo.EntryEmployee", "GetAll");

                // Check if codebooks is not null and contains any elements
                if (codebooks != null && codebooks.Any())
                {
                    return codebooks.First().hrgcod;
                }
                else
                {
                    // Handle the case where codebooks is null or empty
                    return "No codebooks found";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during data retrieval
                Console.WriteLine($"An error occurred: {ex.Message}");
                return "Error occurred during data retrieval";
            }
        }



        public async Task<bool> UpdateCode(codebook code)
        {

            return await _dataAccess.UpdateTransData("3101", "dbo.EntryEmployee", "InsertUpdate", code.hrgcod);
          
        }


        public async Task<bool> DeleteCode(string codebook)
        {
            return await _dataAccess.UpdateTransData("YourCompCode", "DeleteEmployee", "YourCallType","");
        }
    }

}
