using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccess;
using WebApi.DataAccess.Exceptions;
using WebApi.DataAccess.Models;
using WebApi.Services.DTOs;
using WebApi.Services.Exceptions;

namespace WebApi.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            if (teamRepository == null)
            {
                throw new ArgumentNullException("teamRepository");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamDTO>> GetTeams()
        {
            return _mapper.Map<List<TeamDTO>>(await _teamRepository.GetTeams());
        }

        public async Task EditTeam(TeamDTO teamDTO)
        {
            var team = _mapper.Map<Team>(teamDTO);
            try
            {
                await _teamRepository.EditTeam(team);
            }
            catch (EntryDeletedException)
            {
                throw new TeamDeletedException();
            }
            catch (EntryModifiedException)
            {
                throw new TeamModifiedException();
            }
            catch (EntryNotExistedException)
            {
                throw new TeamNotExistedException();
            }
            catch (DatabaseConstraintException)
            {
                throw new TeamNameAlreadyExistsException();
            }
        }

        public async Task<TeamDTO> AddTeam(TeamDTO teamDTO)
        {
            var team = _mapper.Map<Team>(teamDTO);
            try
            {
                return _mapper.Map<TeamDTO>(await _teamRepository.AddTeam(team));
            }
            catch (DatabaseConstraintException)
            {
                throw new TeamNameAlreadyExistsException();
            }
        }

        public async Task<TeamDTO> GetTeamById(int id)
        {
            var team = await _teamRepository.GetTeamById(id);
            if (team != null)
            {
                return _mapper.Map<TeamDTO>(team);
            }

            return null;
        }

        public async Task<TeamDTO> DeleteTeam(int id)
        {
            var deletedTeam = await _teamRepository.DeleteTeam(id);

            if (deletedTeam != null)
            {
                return _mapper.Map<TeamDTO>(deletedTeam);
            }

            return null;
        }
    }
}
