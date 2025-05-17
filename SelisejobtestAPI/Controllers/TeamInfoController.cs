using Microsoft.AspNetCore.Mvc;
using SelisejobtestAPI.Common;
using SoftInterface.DLL;
using SoftModels.Models;

namespace SelisejobtestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamInfoController : Controller
    {
        private readonly ITeamInfo _iteamInfo;

        public TeamInfoController(ITeamInfo iteamInfo)
        {
            _iteamInfo = iteamInfo;
        }

        #region TeamInfo CROUD

        [HttpGet]
        [Route("GetTeamInfo")]
        public IActionResult GetTeamInfo(int pageNumber = 1, int limit = 10, string search = "")
        {
            var data = _iteamInfo.GetTeamInfo(search);
            // pagination
            int totalItems = data.Count();
            IEnumerable<TeamInfo> paginatedData;
            if (limit == 0)
                paginatedData = data.Skip(pageNumber - 1);
            else
                paginatedData = data.Skip((pageNumber - 1) * limit).Take(limit);

            var response = ReturnData.ReturnDataList(data);
            var result = new ListMetaData<TeamInfo>
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
        [Route("GetTeamInfoById")]
        public IActionResult GetTeamInfoById(int id)
        {
            var coa2List = _iteamInfo.GetTeamInfoById(id);
            var response = ReturnData.ReturnDataListById(coa2List);
            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, resultData = coa2List });
        }

        [HttpPost]
        [Route("SaveTeamInfo")]
        public async Task<IActionResult> SaveTeamInfo(TeamInfoInsert obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.TemName))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                TeamInfo model = new TeamInfo();
                if (!string.IsNullOrEmpty(obj.TemName))
                    model.TemName = obj.TemName;
                if (!string.IsNullOrEmpty(obj.Description))
                    model.Description = obj.Description;

                // model.AddedBy = user.Id.ToString();
                model.CreatedDate = DateTime.Now.ToString();
                model.IsActive = "Active";
                var data = await _iteamInfo.SaveTeamInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
            //return BadRequest();
        }

        [HttpPut]
        [Route("UpdateTeamInfo")]
        public async Task<IActionResult> UpdateTeamInfo(TeamInfoInsert obj)
        {
            if (obj.Id > 0)
            {
                TeamInfo model = new TeamInfo();
                model.Id = obj.Id;
                model.IsActive = obj.IsActive;
                if (string.IsNullOrEmpty(obj.TemName))
                    return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });

                if (!string.IsNullOrEmpty(obj.TemName))
                    model.TemName = obj.TemName;

                    model.Description = obj.Description;

                //model.UpdatedBy = user.Id.ToString();
                model.UpdatedDate = DateTime.Now.ToString();
                var data = await _iteamInfo.UpdateTeamInfo(model);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        [HttpDelete]
        [Route("DeleteTeamInfo")]
        public async Task<IActionResult> DeleteTeamInfo(int id)
        {
            if (id > 0)
            {
                var data = await _iteamInfo.DeleteTeamInfo(id);
                return Ok(data);
            }
            else
                return Ok(new { IsSuccess = false, Message = "Sorry Something Wrong!!" });
        }

        #endregion
    }
}
