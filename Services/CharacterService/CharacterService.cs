using dotnet_rpg.Models;
using System.Security.Claims;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor) : ICharacterService
    {
        private readonly IMapper _mapper = mapper;
        private readonly DataContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var Character = _mapper.Map<Character>(character);
                Character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
                _context.Characters.Add(Character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters
                        .Where(c => c.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var DbCharacters = await _context.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .Where(c => c.User!.Id == GetUserId())
                    .ToListAsync();
                serviceResponse.Data = DbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var DbCharacter = await _context.Characters
                    .Include(c => c.Skills) 
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(DbCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var Character = await _context.Characters
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.Id == character.Id);
                if (Character == null || Character.User!.Id != GetUserId())
                {
                    throw new Exception($"Character with Id '{character.Id}' not found!");
                }   

                _mapper.Map(character, Character);
                Character.Name = character.Name;
                Character.HitPoints = character.HitPoints;
                Character.Strength = character.Strength;
                Character.Defense = character.Defense;
                Character.Intelligence = character.Intelligence;
                Character.Class = character.Class;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(Character);
            }
            catch (Exception ex) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var Character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId()) ?? throw new Exception($"Character with Id '{id}' not found!");

                _context.Characters.Remove(Character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                        .Where(c => c.User!.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto characterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == characterSkill.CharacterId && c.User!.Id == GetUserId());
                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }
                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == characterSkill.SkillId);
                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found.";
                    return response;
                }
                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
