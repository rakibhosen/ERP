using ERP.ENTITY.Models.HRM._01.CodeBook;
using ERP.ENTITY.Models.HRM._03.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SERVICE.IRepositories.HRM
{
    public interface ICodeBookReposiroty
    {
        Task<IEnumerable<codebook>> GetCode();
        Task<codebook> GetCodeById(string code);
        Task<string> AddCode(codebook code);
        Task<bool> UpdateCode(codebook code);
        Task<bool> DeleteCode(string code);
    }

}
