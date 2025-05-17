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
    public class UserRoleBLL : IUserRole
    {
        private readonly ConnectionStrings _dbSettings;
        public UserRoleBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region UserRole CRUD

       

        public IEnumerable<UserRole> GetUserRole(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,RoleName,IsActive from UserRole where 1=1  and IsActive='Active' ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and RoleName like '%" + search + "%'  ";
                var models = connection.Query<UserRole>(sql);
                return models;
            }
        }

        public UserRole GetUserRoleById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,RoleName,IsActive from UserRole where 1=1  and IsActive='Active' ";
                if (id > 0)
                {
                    sql += "and id=" + id + "";
                }
                var models = connection.Query<UserRole>(sql.ToString(), new { id = id, }).FirstOrDefault(); 
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveUserRole(UserRole model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUserRole(model.RoleName, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  UserRole");
                    strSql.AppendLine(" (  RoleName,IsActive,CreatedBy,CreatedDate ) VALUES ");
                    strSql.AppendLine(" ( @RoleName,@IsActive,@CreatedBy,@CreatedDate  );");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            RoleName = model.RoleName,
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

        public async Task<DataBaseResponse> UpdateUserRole(UserRole model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateUserRole(model.RoleName, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  UserRole SET ");
                    strSql.AppendLine(" RoleName=@RoleName,IsActive=@IsActive,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            RoleName = model.RoleName,
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
                        //response.Message = exp.Message;
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
        public async Task<DataBaseResponse> DeleteUserRole(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from UserRole where Id= @Id");
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
                   // response.Message = exp.Message;
                    response.IsSuccess = false;
                }
            }
            return response;
        }
        public bool GetDuplicateUserRole(string roleName, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,RoleName,IsActive from UserRole where 1=1  ";
                if (!string.IsNullOrEmpty(roleName))
                    sql += @" and REPLACE(LOWER(RoleName), ' ', '') =REPLACE('" + roleName + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<UserRole>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
