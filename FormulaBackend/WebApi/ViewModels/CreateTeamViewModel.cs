using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Api.ViewModels
{
    public class CreateTeamViewModel
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int? YearOfFoundation { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? WonWorldChampionshipsCount { get; set; }

        [Required]
        public bool? IsEntryFeePaid { get; set; }
    }
}
