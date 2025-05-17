using SoftCore.Configuration;
using SoftInterface.DLL;
using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SoftCore.BLL
{
    public class EmployeeInfoBLL : IEmployeeInfo
    {
        private readonly ConnectionStrings _dbSettings;
        public EmployeeInfoBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        #region Employee CRUD

       

        public IEnumerable<EmployeeInfo> GetEmployeeInfo(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,FullName,Email,IsActive from EmployeeInfo where 1=1 and IsActive='Active' ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and FullName like '%" + search + "%'  ";
                var models = connection.Query<EmployeeInfo>(sql);
                return models;
            }
        }
        public EmployeeInfo GetEmployeeById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,FullName,Email,IsActive from EmployeeInfo where 1=1  and IsActive='Active' ";
                if (id > 0)
                {
                    sql += "and id=" + id + "";
                }
                var models = connection.Query<EmployeeInfo>(sql.ToString(), new { id = id, }).FirstOrDefault();
                return models;
            }
        }
        public async Task<DataBaseResponse> SaveEmployeeInfo(EmployeeInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateEmployeeInfo(model.Email, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  EmployeeInfo");
                    strSql.AppendLine(" (  FullName,Email,IsActive,CreatedDate,CreatedBy ) VALUES ");
                    strSql.AppendLine(" ( @FullName,@Email,@IsActive,@CreatedDate,@CreatedBy );");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            FullName = model.FullName,
                                            Email = model.Email,
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
    

        public async Task<DataBaseResponse> UpdateEmployeeInfo(EmployeeInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateEmployeeInfo(model.Email, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  EmployeeInfo SET ");
                    strSql.AppendLine("FullName=@FullName,Email=@Email,IsActive=@IsActive,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            FullName = model.FullName,
                                            Email = model.Email,
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
                        response.Message = exp.Message;
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
        public async Task<DataBaseResponse> DeleteEmployeeInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from EmployeeInfo where Id= @Id");
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

        public bool GetDuplicateEmployeeInfo(string email, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,FullName,Email from EmployeeInfo where 1=1  ";
                if (!string.IsNullOrEmpty(email))
                    sql += @" and REPLACE(LOWER(Email), ' ', '') =REPLACE('" + email + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<EmployeeInfo>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}
