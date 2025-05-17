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
    public class TaskInfoBLL : ITaskInfo
    {
        private readonly ConnectionStrings _dbSettings;
        public TaskInfoBLL(ConnectionStrings dbSettings)
        {
            _dbSettings = dbSettings;
        }


        #region


        public IEnumerable<TaskInfo> GetTaskInfo(string search)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Title,Description,Status,UserId,TeamId,DueDate,IsActive from TaskInfo where 1=1  and IsActive='Active' ";
                if (!string.IsNullOrEmpty(search))
                    sql += " and Title like '%" + search + "%'  ";
                var models = connection.Query<TaskInfo>(sql);
                return models;
            }
        }

        public TaskInfo GetTaskInfoById(int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @"Select Id,Title,Description,Status,UserId,TeamId,DueDate,IsActive from TaskInfo where 1=1  and IsActive='Active' ";
                if (id > 0)
                {
                    sql += "and id=" + id + "";
                }
                var models = connection.Query<TaskInfo>(sql.ToString(), new { id = id, }).FirstOrDefault();
                return models;
            }
        }

        public async Task<DataBaseResponse> SaveTaskInfo(TaskInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" INSERT INTO  TaskInfo");
                strSql.AppendLine(" (  Title,Description,Status,UserId,TeamId,DueDate,IsActive,CreatedBy,CreatedDate ) VALUES ");
                strSql.AppendLine(" ( @Title,@Description,@Status,@UserId,@TeamId,@DueDate,@IsActive,@CreatedBy,@CreatedDate  );");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Title = model.Title,
                                        Description = model.Description,
                                        Status = model.Status,
                                        UserId = model.UserId,
                                        TeamId = model.TeamId,
                                        DueDate = model.DueDate,
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
            return response;
        }

        public async Task<DataBaseResponse> UpdateTaskInfo(TaskInfo model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  TaskInfo SET ");
                strSql.AppendLine(" Title=@Title,Description=@Description,Status=@Status,UserId=@UserId,TeamId=@TeamId,DueDate=@DueDate,IsActive=@IsActive,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Title = model.Title,
                                        Description = model.Description,
                                        Status = model.Status,
                                        UserId = model.UserId,
                                        TeamId = model.TeamId,
                                        DueDate = model.DueDate,
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
            return response;
        }

        public async Task<DataBaseResponse> DeleteTaskInfo(int id)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" Delete from TaskInfo where Id= @Id");
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

        public async Task<DataBaseResponse> UpdateTaskStatus(TaskStatusUpdateInsert model)
        {
            var response = new DataBaseResponse();
            await using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine(" UPDATE  TaskInfo SET ");
                strSql.AppendLine("Status=@Status,DueDate=@DueDate,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy ");
                strSql.AppendLine(" Where Id=@Id;");
                try
                {
                    var Saveresult = connection.Execute(strSql.ToString(),
                                    new
                                    {
                                        Id = model.Id,
                                        Status = model.Status,
                                        DueDate = model.DueDate
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
            return response;
        }



        #endregion
    }
}
