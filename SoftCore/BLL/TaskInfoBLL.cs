using Dapper;
using Microsoft.Data.SqlClient;
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
            throw new NotImplementedException();
        }

        public TaskInfo GetTaskInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> SaveTaskInfo(TaskInfo model)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> UpdateTaskInfo(TaskInfo model)
        {
            throw new NotImplementedException();
        }

        public Task<DataBaseResponse> DeleteTaskInfo(int id)
        {
            throw new NotImplementedException();
        }

        public bool GetDuplicateTaskInfo(string temName, int id)
        {
            using (var connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var sql = @" Select Id,TemName,Description,IsActive from TeamInfo where 1=1  ";
                if (!string.IsNullOrEmpty(temName))
                    sql += @" and REPLACE(LOWER(TemName), ' ', '') =REPLACE('" + temName + @"', ' ', '') ";
                if (id > 0)
                    sql += @" and Id!=" + id + " ";

                var models = connection.Query<TaskInfo>(sql);
                if (models.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
