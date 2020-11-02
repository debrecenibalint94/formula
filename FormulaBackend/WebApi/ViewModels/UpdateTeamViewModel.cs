using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Api.ViewModels
{
    public class UpdateTeamViewModel
    {
        [Required]
        public TeamViewModel Original { get; set; }
        
        [Required]
        public TeamViewModel Updated { get; set; }
    }
}
