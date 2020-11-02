using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess.Models;

namespace WebApi.DataAccess
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByUserNameAndPassword(string userName, string password);
    }
}
