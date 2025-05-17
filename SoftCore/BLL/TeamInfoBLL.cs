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
    public class TeamInfoBLL: ITeamInfo
    {
        private readonly ConnectionStrings _dbSettings;
        public TeamInfoBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }


        #region TeamInfo CRUD
        

        public IEnumerable<TeamInfo> GetTeamInfo(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,TemName,Description,IsActive from TeamInfo where 1=1  and IsActive='Active' ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and TemName like '%" + search + "%'  ";
                var models = connection.Query<TeamInfo>(sql);
                return models;
            }
        }

        public TeamInfo GetTeamInfoById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,TemName,Description,IsActive from TeamInfo where 1=1  and IsActive='Active' ";
                if (id > 0)
                {
                    sql += "and id=" + id + "";
                }
                var models = connection.Query<TeamInfo>(sql.ToString(), new { id = id, }).FirstOrDefault(); 
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveTeamInfo(TeamInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateTeamInfo(model.TemName, 0);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" INSERT INTO  TeamInfo");
                    strSql.AppendLine(" (  TemName,Description,IsActive,CreatedBy,CreatedDate ) VALUES ");
                    strSql.AppendLine(" ( @TemName,@Description,@IsActive,@CreatedBy,@CreatedDate  );");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            TemName = model.TemName,
                                            Description = model.Description,
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

        public async Task<DataBaseResponse> UpdateTeamInfo(TeamInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                bool duplicateresult = GetDuplicateTeamInfo(model.TemName, model.Id);
                if (duplicateresult)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendLine(" UPDATE  TeamInfo SET ");
                    strSql.AppendLine(" TemName=@TemName,Description=@Description,IsActive=@IsActive,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                    strSql.AppendLine(" Where Id=@Id;");
                    try
                    {
                        var Saveresult = connection.Execute(strSql.ToString(),
                                        new
                                        {
                                            Id = model.Id,
                                            TemName = model.TemName,
                                            Description = model.Description,
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

        public async Task<DataBaseResponse> DeleteTeamInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from TeamInfo where Id= @Id");
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
        public bool GetDuplicateTeamInfo(string temName, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,TemName,Description,IsActive from TeamInfo where 1=1  ";
                if (!string.IsNullOrEmpty(temName))
                    sql += @" and REPLACE(LOWER(TemName), ' ', '') =REPLACE('" + temName + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<TeamInfo>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}
