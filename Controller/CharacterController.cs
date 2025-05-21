using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_rpg.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController(ICharacterService characterService) : ControllerBase
    {
        private readonly ICharacterService _characterService = characterService;

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            try
            {
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
                var result = await _characterService.GetAllCharacters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<List<GetCharacterDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetSingle(int id)
        {
            try
            {
                var result = await _characterService.GetCharacter(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<GetCharacterDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpPost("AddCharacter")]
        public async Task<ActionResult<ServiceResponse<List<AddCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            try
            {
                var result = await _characterService.AddCharacter(newCharacter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<List<AddCharacterDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpPut("UpdateCharacter")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            try
            {
                var response = await _characterService.UpdateCharacter(updateCharacter);
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<List<AddCharacterDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            try
            {
                var response = await _characterService.DeleteCharacter(id);
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<GetCharacterDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            try
            {
                var response = await _characterService.AddCharacterSkill(newCharacterSkill);
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResponse<GetCharacterDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
