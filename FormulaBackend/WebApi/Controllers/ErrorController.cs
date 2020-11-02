using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public ErrorController()
        {

        }

        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
