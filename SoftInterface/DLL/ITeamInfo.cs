using SoftInterface.ResponseModel;
using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface ITeamInfo
    {
        IEnumerable<TeamInfo> GetTeamInfo(string search);
        TeamInfo GetTeamInfoById(int id);
        Task<DataBaseResponse> SaveTeamInfo(TeamInfo model);
        Task<DataBaseResponse> UpdateTeamInfo(TeamInfo model);
        Task<DataBaseResponse> DeleteTeamInfo(int id);
    }
}
