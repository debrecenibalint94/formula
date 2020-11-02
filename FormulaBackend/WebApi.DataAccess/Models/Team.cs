using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccess.Data;

namespace WebApi.DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int YearOfFoundation { get; set; }
        [ConcurrencyCheck]
        public int WonWorldChampionshipsCount { get; set; }
        [ConcurrencyCheck]
        public bool IsEntryFeePaid { get; set; }
        [NotMapped]
        public int OriginalWonWorldChampionshipsCount { get; set; }
        [NotMapped]
        public bool OriginalIsEntryFeePaid { get; set; }

        public void UpdateWonWorldChampionshipsCount(FormulaContext context, int orgWonWorldChampionshipsCount, int newWonWorldChampionshipsCount)
        {
            WonWorldChampionshipsCount = newWonWorldChampionshipsCount;
            context.Entry(this).Property(p => p.WonWorldChampionshipsCount).OriginalValue = orgWonWorldChampionshipsCount;
        }

        public void UpdateIsEntryFeePaid(FormulaContext context, bool orgIsEntryFeePaid, bool newIsEntryFeePaid)
        {
            IsEntryFeePaid = newIsEntryFeePaid;
            context.Entry(this).Property(p => p.IsEntryFeePaid).OriginalValue = orgIsEntryFeePaid;
        }
    }
}
