using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface ITaskInfo
    {
        IEnumerable<TaskInfo> GetTaskInfo(string search);
        TaskInfo GetTaskInfoById(int id);
        Task<DataBaseResponse> SaveTaskInfo(TaskInfo model);
        Task<DataBaseResponse> UpdateTaskInfo(TaskInfo model);
        Task<DataBaseResponse> DeleteTaskInfo(int id);

        Task<DataBaseResponse> UpdateTaskStatus(TaskStatusUpdateInsert model);
    }
}
