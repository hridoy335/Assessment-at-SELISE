using Azure;
using Microsoft.AspNetCore.Mvc;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeInfoController : Controller
    {
        private readonly IEmployeeInfo _iemployeeInfo;
        public EmployeeInfoController(IEmployeeInfo employeeInfo)
        {
            _iemployeeInfo = employeeInfo;
        }

        #region EmployeeInfo CROUD

       
        [HttpGet]
        [Route("GetEmployeeInfo")]
        public IActionResult GetEmployeeInfo(int pageNumber = 1, int limit = 10, string search = "")
        {
            var data = _iemployeeInfo.GetEmployeeInfo(search);
            // pagination
            int totalItems = data.Count();
            IEnumerable<EmployeeInfo> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<EmployeeInfo>
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
            var coa2List = _iemployeeInfo.GetEmployeeById(id);
            var response = ReturnData.ReturnDataListById(coa2List);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = coa2List });
        }

        [HttpPost]
        [Route("SaveEmployeeInfo")]
        public async Task<IActionResult> SaveEmployeeInfo(EmployeeInfoInsert obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.FullName) || string.IsNullOrEmpty(obj.Email))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                EmployeeInfo model = new EmployeeInfo();
                if (!string.IsNullOrEmpty(obj.FullName))
                    model.FullName = obj.FullName;
                if (!string.IsNullOrEmpty(obj.Email))
                    model.Email = obj.Email;

                // model.AddedBy = user.Id.ToString();
                model.CreatedDate = DateTime.Now.ToString();
                model.IsActive = "Active";
                var data = await _iemployeeInfo.SaveEmployeeInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!"});
                //return BadRequest();
        }

        [HttpPut]
        [Route("UpdateEmployeeInfo")]
        public async Task<IActionResult> UpdateEmployeeInfo(EmployeeInfoInsert obj)
        {
            if (obj.Id > 0)
            {
                EmployeeInfo model = new EmployeeInfo();
                model.Id = obj.Id;
                model.IsActive = obj.IsActive;
                if (string.IsNullOrEmpty(obj.FullName) || string.IsNullOrEmpty(obj.Email))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.FullName))
                    model.FullName = obj.FullName;
                if (!string.IsNullOrEmpty(obj.Email))
                    model.Email = obj.Email;

                //model.UpdatedBy = user.Id.ToString();
                model.UpdatedDate = DateTime.Now.ToString();
                var data = await _iemployeeInfo.UpdateEmployeeInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpDelete]
        [Route("DeleteEmployeeInfo")]
        public async Task<IActionResult> DeleteEmployeeInfo(int id)
        {
            if (id > 0)
            {
                var data = await _iemployeeInfo.DeleteEmployeeInfo(id);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        #endregion
    }
}
