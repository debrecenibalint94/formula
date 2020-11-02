using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Services.DTOs;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByUserNameAndPassword(string userName, string password);
    }
}
