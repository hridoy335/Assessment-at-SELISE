using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskInfoController : Controller
    {
        private readonly ITaskInfo _taskInfo;

        public TaskInfoController(ITaskInfo taskInfo)
        {
            _taskInfo = taskInfo;
        }

        #region
        [HttpGet]
        [Route("GetTaskInfo")]
        public IActionResult GetTaskInfo(int pageNumber = 1, int limit = 10, string search = "")
        {

            var data = _taskInfo.GetTaskInfo(search);
            // pagination
            int totalItems = data.Count();
            IEnumerable<TaskInfo> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<TaskInfo>
            {
                TotalData = data.Count(),
                DataFound = paginatedData.Count(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)data.Count() / limit),
                DataLimit = limit,
                Data = paginatedData
            };
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = result });

        }

        [HttpGet]
        [Route("GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            var coa2List = _taskInfo.GetTaskInfoById(id);
            var response = ReturnData.ReturnDataListById(coa2List);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = coa2List });
        }

        [HttpPost]
        [Route("SaveTaskInfo")]
        public async Task<IActionResult> SaveTaskInfo(TaskInfoInsert obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.Title) || string.IsNullOrEmpty(obj.Description) || string.IsNullOrEmpty(obj.Status)  || obj.UserId<0 || obj.TeamId<0)
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                TaskInfo model = new TaskInfo();
                if (!string.IsNullOrEmpty(obj.Title))
                    model.Title = obj.Title;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;
                if (!string.IsNullOrEmpty(obj.Status))
                    model.Status = obj.Status;
                if(obj.UserId>0)
                    model.UserId= obj.UserId;
                if (obj.TeamId > 0)
                    model.TeamId = obj.TeamId;

                // model.AddedBy = user.Id.ToString();
                model.CreatedDate = DateTime.Now.ToString();
                model.IsActive = "Active";
                var data = await _taskInfo.SaveTaskInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
            //return BadRequest();
        }

        [HttpPut]
        [Route("UpdateTaskInfo")]
        public async Task<IActionResult> UpdateTaskInfo(TaskInfoInsert obj)
        {
            if (obj.Id > 0)
            {
                TaskInfo model = new TaskInfo();
                model.Id = obj.Id;
                model.IsActive = obj.IsActive;
                if (string.IsNullOrEmpty(obj.Title) || string.IsNullOrEmpty(obj.Description) || string.IsNullOrEmpty(obj.Status) || obj.UserId < 0 || obj.TeamId < 0)
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.Title))
                    model.Title = obj.Title;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;
                if (!string.IsNullOrEmpty(obj.Status))
                    model.Status = obj.Status;
                if (obj.UserId > 0)
                    model.UserId = obj.UserId;
                if (obj.TeamId > 0)
                    model.TeamId = obj.TeamId;

                //model.UpdatedBy = user.Id.ToString();
                model.UpdatedDate = DateTime.Now.ToString();
                var data = await _taskInfo.UpdateTaskInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpDelete]
        [Route("DeleteTaskInfo")]
        public async Task<IActionResult> DeleteTaskInfo(int id)
        {
            if (id > 0)
            {
                var data = await _taskInfo.DeleteTaskInfo(id);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }


        [HttpPut]
        [Route("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(TaskStatusUpdateInsert obj)
        {
            if (obj.Id > 0)
            {
                TaskStatusUpdateInsert model = new TaskStatusUpdateInsert();
                model.Id = obj.Id;
                if ( string.IsNullOrEmpty(obj.Status) || string.IsNullOrEmpty(obj.DueDate))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.DueDate))
                    model.DueDate = obj.DueDate;
                if (!string.IsNullOrEmpty(obj.Status))
                    model.Status = obj.Status;

                var data = await _taskInfo.UpdateTaskStatus(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        #endregion
    }
}
