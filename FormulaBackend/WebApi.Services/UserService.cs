using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess;
using WebApi.Services.DTOs;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> GetUserByUserNameAndPassword(string userName, string password)
        {
            var user = await _userRepository.GetUserByUserNameAndPassword(userName, password);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }

            return null;
        }
    }
}
