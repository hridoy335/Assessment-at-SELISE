using Dapper;
using Microsoft.Data.SqlClient;
using SoftCore.Configuration;
using SoftInterface.DLL;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEng.Core.BLL
{
    public class UserLoginBLL : IUserLogin
    {
        private readonly ConnectionStrings _dbSettings;
        public UserLoginBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public UserInfo CheckUserLogin(string username, string password)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select u.Id,u.EmployeeId,u.RoleId,u.IsActive, 
                      e.FullName,r.RoleName
                      from UserInfo u
                      left join EmployeeInfo e on e.id=u.EmployeeId
                      left join UserRole r on r.Id=u.RoleId
                      where 1=1 and u.IsActive='Active' ";
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    sql += "and u.UserName='" + username + "' and u.Password='"+password+"' ";
                }
                var models = connection.Query<UserInfo>(sql.ToString()).FirstOrDefault();
                return models;
            }
        }
    }
}
