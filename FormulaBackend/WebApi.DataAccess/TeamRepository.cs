using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.Data;
using WebApi.DataAccess.Exceptions;
using WebApi.DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace WebApi.DataAccess
{
    public class TeamRepository : ITeamRepository
    {
        private readonly FormulaContext _context;
        public TeamRepository(FormulaContext formulaContext)
        {
            if (formulaContext == null)
            {
                throw new ArgumentNullException("formulaContext");
            }
            _context = formulaContext;
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await _context.Teams.AsNoTracking().ToListAsync();
        }

        public async Task EditTeam(Team team)
        {
            var modifyingTeam = await _context.Teams.FindAsync(team.Id);
            if (modifyingTeam != null)
            {
                modifyingTeam.Name = team.Name;
                modifyingTeam.YearOfFoundation = team.YearOfFoundation;

                modifyingTeam.UpdateWonWorldChampionshipsCount(_context, team.OriginalWonWorldChampionshipsCount, team.WonWorldChampionshipsCount);
                modifyingTeam.UpdateIsEntryFeePaid(_context, team.OriginalIsEntryFeePaid, team.IsEntryFeePaid);

                await TrySave(modifyingTeam);
            }
            else
            {
                throw new EntryNotExistedException();
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        public async Task<Team> AddTeam(Team team)
        {
            _context.Teams.Add(team);
            await TrySave(team);
            return team;
        }

        public async Task<Team> GetTeamById(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            return team;
        }

        public async Task<Team> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return null;
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return team;
        }

        private async Task TrySave(Team team)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(team.Id))
                {
                    throw new EntryDeletedException();
                }
                else
                {
                    throw new EntryModifiedException();
                }
            }
            catch (DbUpdateException e)
                when (e.InnerException is SqliteException sqliteException
                && sqliteException.SqliteErrorCode == (int)SqliteErrorCode.SqliteConstraint
                && sqliteException.SqliteExtendedErrorCode == (int)SqliteExtendedErrorCode.SqliteConstraintUnique)
            {
                var matches = Regex.Matches(sqliteException.Message, @"(\w+)\.(\w+)", RegexOptions.IgnoreCase);

                throw new DatabaseConstraintException()
                {
                    Table = matches[0].Groups[1].Value,
                    Column = matches[0].Groups[2].Value
                };
            }
            catch (DbUpdateException)
            {
                if (!TeamExists(team.Id))
                {
                    throw new EntryNotExistedException();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
