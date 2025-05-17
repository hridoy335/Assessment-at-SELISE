using SoftModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInterface.DLL
{
    public interface IUserLogin
    {
        UserInfo CheckUserLogin(string username, string password);
    }
}
