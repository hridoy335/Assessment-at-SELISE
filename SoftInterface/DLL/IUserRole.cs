using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface IUserRole
    {
        IEnumerable<UserRole> GetUserRole(string search);
        UserRole GetUserRoleById(int id);
        Task<DataBaseResponse> SaveUserRole(UserRole model);
        Task<DataBaseResponse> UpdateUserRole(UserRole model);
        Task<DataBaseResponse> DeleteUserRole(int id);
    }
}
