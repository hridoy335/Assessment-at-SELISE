using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface IEmployeeInfo
    {
        IEnumerable<EmployeeInfo> GetEmployeeInfo(string search);
        EmployeeInfo GetEmployeeById(int id);
        Task<DataBaseResponse> SaveEmployeeInfo(EmployeeInfo model);
        Task<DataBaseResponse> UpdateEmployeeInfo(EmployeeInfo model);
        Task<DataBaseResponse> DeleteEmployeeInfo(int id);
    }
}
