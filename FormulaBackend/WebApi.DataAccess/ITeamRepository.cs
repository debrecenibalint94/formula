using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess.Models;

namespace WebApi.DataAccess
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeams();
        Task EditTeam(Team team);
        Task<Team> AddTeam(Team team);
        Task<Team> GetTeamById(int id);
        Task<Team> DeleteTeam(int id);
    }
}
