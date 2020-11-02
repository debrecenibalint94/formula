using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Services.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
