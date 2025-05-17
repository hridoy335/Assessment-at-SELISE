using Dapper;
using Microsoft.Data.SqlClient;
using SoftCore;
using SoftCore.Configuration;
using SoftInterface.DLL;
using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEng.Core.BLL
{
    public class UserInfoBLL: IUserInfo
    {
        private readonly ConnectionStrings _dbSettings;
        public UserInfoBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }


        #region UserInfo CRUD
        public IEnumerable<UserInfo> GetUserInfo(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"  Select u.Id,u.EmployeeId,u.RoleId,u.IsActive, 
                      e.FullName,r.RoleName
                      from UserInfo u
                      left join EmployeeInfo e on e.id=u.EmployeeId
                      left join UserRole r on r.Id=u.RoleId
                      where 1=1 and u.IsActive='Active' ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and e.FullName like '%" + search + "%'  ";
                var models = connection.Query<UserInfo>(sql);
                return models;
            }
        }

        public UserInfo GetUserInfoById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select u.Id,u.EmployeeId,u.RoleId,u.IsActive, 
                      e.FullName,r.RoleName
                      from UserInfo u
                      left join EmployeeInfo e on e.id=u.EmployeeId
                      left join UserRole r on r.Id=u.RoleId
                      where 1=1 and u.IsActive='Active' ";
                if (id > 0)
                {
                    sql += "and u.id=" + id + "";
                }
                var models = connection.Query<UserInfo>(sql.ToString(), new { id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveUserInfo(UserInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUserInfo(model.UserName, 0,model.EmployeeId);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  UserInfo");
                    strSql.AppendLine(" (  EmployeeId,RoleId,UserName,Password,IsActive,CreatedBy,CreatedDate ) VALUES ");
                    strSql.AppendLine(" ( @EmployeeId,@RoleId,@UserName,@Password,@IsActive,@CreatedBy,@CreatedDate  );");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            EmployeeId = model.EmployeeId,
                                            RoleId = model.RoleId,
                                            UserName = model.UserName,
                                            Password = model.Password,
                                            IsActive = model.IsActive,
                                            CreatedDate = model.CreatedDate,
                                            CreatedBy = model.CreatedBy,
                                        });
                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.INSERT_SUCCESS_MESSAGE;
                        response.IsSuccess = true;
                    }
                    catch (Exception exp)
                    {
                        response.ReturnValue = -1;
                        response.Message = "Something Wrong!!";
                       // response.Message = exp.Message;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }

        public async Task<DataBaseResponse> UpdateUserInfo(UserInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUserInfo(model.UserName, model.Id,model.EmployeeId);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  UserInfo SET ");
                    strSql.AppendLine(" EmployeeId=@EmployeeId,RoleId=@RoleId,UserName=@UserName,Password=@Password,IsActive=@IsActive,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            EmployeeId = model.EmployeeId,
                                            RoleId = model.RoleId,
                                            UserName = model.UserName,
                                            Password = model.Password,
                                            IsActive = model.IsActive,
                                            UpdatedDate = model.UpdatedDate,
                                            UpdatedBy = model.UpdatedBy,
                                        }
                                        );
                        response.ReturnValue = Saveresult;
                        response.Message = GlobalConst.UPDATE_SUCCESS_MESSAGE;
                        response.IsSuccess = true;
                    }
                    catch (Exception exp)
                    {
                        response.ReturnValue = -1;
                        response.Message = "Something Wrong!!";
                        // response.Message = exp.Message;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    response.ReturnValue = -1;
                    response.Message = GlobalConst.GET_DUPLICATEDATA;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public async Task<DataBaseResponse> DeleteUserInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from UserInfo where Id= @Id");
                try
                {
                    var result = connection.Execute(strSql.ToString(), new { Id = id, });
                    response.ReturnValue = result;
                    response.Message = GlobalConst.DELETE_SUCCESS_MESSAGE;
                    response.IsSuccess = true;
                }
                catch (Exception exp)
                {
                    response.ReturnValue = -1;
                    response.Message = "Something Wrong!!";
                    //response.Message = exp.Message;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public bool GetDuplicateUserInfo(string userName, int id, int EmployeeId )
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,EmployeeId,RoleId,UserName,Password,IsActive from UserInfo where 1=1  ";
                if (!string.IsNullOrEmpty(userName))
                    sql += @" and REPLACE(LOWER(UserName), ' ', '') =REPLACE('" + userName + @"', ' ', '') ";
                if (EmployeeId > 0)
                    sql += @" and EmployeeId!=" + EmployeeId + " ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<UserInfo>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }


        #endregion
    }
}
