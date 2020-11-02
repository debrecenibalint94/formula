using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;
using WebApi.Api.ViewModels;
using WebApi.Services.DTOs;
using WebApi.Services.Exceptions;
using System.Net;

namespace WebApi.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public TeamsController(ITeamService teamService, IMapper mapper)
        {
            if (teamService == null)
            {
                throw new ArgumentNullException("teamService");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            _teamService = teamService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamViewModel>>> GetTeams()
        {
            return _mapper.Map<List<TeamViewModel>>(await _teamService.GetTeams());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamViewModel>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamById(id);

            if (team == null)
            {
                return NotFound();
            }

            return _mapper.Map<TeamViewModel>(team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, UpdateTeamViewModel updateTeamViewModel)
        {
            if (id != updateTeamViewModel.Updated.Id)
            {
                return BadRequest();
            }

            try
            {
                await _teamService.EditTeam(_mapper.Map<TeamDTO>(updateTeamViewModel));
            }
            catch (TeamDeletedException)
            {
                return Problem(detail: "The team has been deleted by other user");
            }
            catch (TeamModifiedException)
            {
                return Problem(detail: "The team has been modified by other user");
            }
            catch (TeamNotExistedException)
            {
                return Problem(detail: "The team does not exist");
            }
            catch (TeamNameAlreadyExistsException)
            {
                ModelState.AddModelError("Name", "A team already exists with this name");
                return ValidationProblem(statusCode: (int)HttpStatusCode.Conflict, modelStateDictionary: ModelState);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TeamViewModel>> PostTeam(CreateTeamViewModel createTeamViewModel)
        {
            var teamDTO = _mapper.Map<TeamDTO>(createTeamViewModel);
            try
            {
                await _teamService.AddTeam(teamDTO);
                var team = _mapper.Map<TeamViewModel>(teamDTO);

                return CreatedAtAction("GetTeam", new { id = team.Id }, team);
            }
            catch (TeamNameAlreadyExistsException)
            {
                ModelState.AddModelError("Name", "A team already exists with this name");
                return ValidationProblem(statusCode: (int)HttpStatusCode.Conflict, modelStateDictionary: ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamViewModel>> DeleteTeam(int id)
        {
            var team = await _teamService.DeleteTeam(id);

            if (team == null)
            {
                return NotFound();
            }

            return _mapper.Map<TeamViewModel>(team);
        }
    }
}
