using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Api.ViewModels
{
    public class AuthenticationViewModel
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
