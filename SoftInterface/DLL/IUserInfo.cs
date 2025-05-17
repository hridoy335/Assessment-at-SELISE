using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface IUserInfo
    {
        IEnumerable<UserInfo> GetUserInfo(string search);
        UserInfo GetUserInfoById(int id);
        Task<DataBaseResponse> SaveUserInfo(UserInfo model);
        Task<DataBaseResponse> UpdateUserInfo(UserInfo model);
        Task<DataBaseResponse> DeleteUserInfo(int id);
    }
}
