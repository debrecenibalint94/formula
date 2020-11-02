using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess.Data;
using WebApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApi.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly FormulaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(FormulaContext formulaContext, UserManager<ApplicationUser> userManager)
        {
            _context = formulaContext;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserByUserNameAndPassword(string userName, string password)
        {
            var searchedUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            if (searchedUser != null)
            {
                var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(searchedUser, searchedUser.PasswordHash, password);
                if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return searchedUser;
                }
            }

            return null;
        }
    }
}
