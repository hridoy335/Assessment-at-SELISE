using Microsoft.AspNetCore.Mvc;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController : Controller
    {
        private readonly IUserRole _userRole;

        public UserRoleController(IUserRole userRole)
        {
            _userRole = userRole;
        }

        #region UserRole CRUD
        [HttpGet]
        [Route("GetUserRole")]
        public IActionResult GetUserRole(int pageNumber = 1, int limit = 10, string search = "")
        {
            var data = _userRole.GetUserRole(search);
            // pagination
            int totalItems = data.Count();
            IEnumerable<UserRole> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<UserRole>
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
        [Route("GetUserRoleById")]
        public IActionResult GetUserRoleById(int id)
        {
            var data = _userRole.GetUserRoleById(id);
            var response = ReturnData.ReturnDataListById(data);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = data });
        }

        [HttpPost]
        [Route("SaveUserRole")]
        public async Task<IActionResult> SaveUserRole(UserRoleInsert obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.RoleName))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                UserRole model = new UserRole();
                if (!string.IsNullOrEmpty(obj.RoleName))
                    model.RoleName = obj.RoleName;
               
                // model.AddedBy = user.Id.ToString();
                model.CreatedDate = DateTime.Now.ToString();
                model.IsActive = "Active";
                var data = await _userRole.SaveUserRole(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpPut]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UserRoleInsert obj)
        {
            if (obj.Id > 0)
            {
                UserRole model = new UserRole();
                model.Id = obj.Id;
                model.IsActive = obj.IsActive;
                if (string.IsNullOrEmpty(obj.RoleName))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.RoleName))
                    model.RoleName = obj.RoleName;

                //model.UpdatedBy = user.Id.ToString();
                model.UpdatedDate = DateTime.Now.ToString();
                var data = await _userRole.UpdateUserRole(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpDelete]
        [Route("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            if (id > 0)
            {
                var data = await _userRole.DeleteUserRole(id);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }


        #endregion
    }
}
