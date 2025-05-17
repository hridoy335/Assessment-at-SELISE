using Azure;
using Microsoft.AspNetCore.Mvc;
using SoftModels.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SelisejobtestAPI.Common
{
    public class TokenVerify : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenVerify(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserInfo GetCurrentUser()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserInfo
                {
                    FullName = userClaims.FirstOrDefault(x => x.Type == "FullName")?.Value,
                    Id = Convert.ToInt32((userClaims.FirstOrDefault(x => x.Type == "Id")?.Value)),
                };
            }

            return null;
        }
       

    }
}
