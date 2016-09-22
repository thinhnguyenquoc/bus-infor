using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface IUserService
    {
        bool CheckLogin(string email, string pass);
    }
}
