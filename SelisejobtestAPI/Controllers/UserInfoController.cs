using Microsoft.AspNetCore.Mvc;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : Controller
    {
        private readonly IUserInfo _userInfo;

        public UserInfoController(IUserInfo userInfo)
        {
            _userInfo = userInfo;
        }


        #region UserInfo CROUD

        [HttpGet]
        [Route("GetUserInfo")]
        public IActionResult GetUserInfo(int pageNumber = 1, int limit = 10, string search = "")
        {
            var data = _userInfo.GetUserInfo(search);
            // pagination
            int totalItems = data.Count();
            IEnumerable<UserInfo> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<UserInfo>
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
        [Route("GetUserInfoById")]
        public IActionResult GetUserInfoById(int id)
        {
            var data = _userInfo.GetUserInfoById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [HttpPost]
        [Route("SaveUserInfo")]
        public async Task<IActionResult> SaveUserInfo(UserInfoInsert obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.UserName) || string.IsNullOrEmpty(obj.Password) || obj.EmployeeId <= 0 || obj.RoleId<=0)
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                UserInfo model = new UserInfo();
                if (!string.IsNullOrEmpty(obj.UserName))
                    model.UserName = obj.UserName;
                if (!string.IsNullOrEmpty(obj.Password))
                    model.Password = obj.Password;
                if(obj.EmployeeId > 0)
                    model.EmployeeId = obj.EmployeeId;
                if (obj.RoleId > 0)
                    model.RoleId = obj.RoleId;
                // model.AddedBy = user.Id.ToString();
                model.CreatedDate = DateTime.Now.ToString();
                model.IsActive = "Active";
                var data = await _userInfo.SaveUserInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpPut]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo(UserInfoInsert obj)
        {
            if (obj.Id > 0)
            {
                UserInfo model = new UserInfo();
                model.Id = obj.Id;
                model.IsActive = obj.IsActive;
                if (string.IsNullOrEmpty(obj.UserName) || string.IsNullOrEmpty(obj.Password) || obj.EmployeeId <= 0 || obj.RoleId <= 0)
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.UserName))
                    model.UserName = obj.UserName;
                if (!string.IsNullOrEmpty(obj.Password))
                    model.Password = obj.Password;
                if (obj.EmployeeId > 0)
                    model.EmployeeId = obj.EmployeeId;
                if (obj.RoleId > 0)
                    model.RoleId = obj.RoleId;

                //model.UpdatedBy = user.Id.ToString();
                model.UpdatedDate = DateTime.Now.ToString();
                var data = await _userInfo.UpdateUserInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpDelete]
        [Route("DeleteUserInfo")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            if (id > 0)
            {
                var data = await _userInfo.DeleteUserInfo(id);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        #endregion
    }
}
