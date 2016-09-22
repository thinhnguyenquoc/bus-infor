using Bus_Info.Entities;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _IUserRepository;
        public UserService(IUserRepository IUserRepository)
        {
            _IUserRepository = IUserRepository;
        }

        public bool CheckLogin(string email, string pass){
            User user = _IUserRepository.All.Where(x=>x.Email == email && x.Pass == pass).FirstOrDefault();
            if(user != null)
                return true;
            else
                return false;
        }
    }
}
