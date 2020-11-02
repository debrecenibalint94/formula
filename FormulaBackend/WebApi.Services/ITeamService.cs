using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.DTOs;

namespace WebApi.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDTO>> GetTeams();
        Task EditTeam(TeamDTO teamDTO);

        Task<TeamDTO> AddTeam(TeamDTO teamDTO);

        Task<TeamDTO> GetTeamById(int id);

        Task<TeamDTO> DeleteTeam(int id);
    }
}
