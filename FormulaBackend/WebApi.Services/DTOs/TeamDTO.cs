using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Services.DTOs
{
    public class TeamDTO
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int YearOfFoundation { get; set; }
            public int WonWorldChampionshipsCount { get; set; }
            public bool IsEntryFeePaid { get; set; }
            public int OriginalWonWorldChampionshipsCount { get; set; }
            public bool OriginalIsEntryFeePaid { get; set; }
    }
}
