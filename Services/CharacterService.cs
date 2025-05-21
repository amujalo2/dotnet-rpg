
using dotnet_rpg.Models;

namespace dotnet_rpg.Services
{
    public class CharacterService(IMapper mapper, DataContext context) : ICharacterService
    {
        private readonly IMapper _mapper = mapper;
        private readonly DataContext _context = context;


        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var Character = _mapper.Map<Character>(character);
                _context.Characters.Add(Character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var DbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
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
                var DbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
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
                var Character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id) ?? throw new Exception($"Character with Id '{character.Id}' not found!");

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
                var Character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Character with Id '{id}' not found!");

                _context.Characters.Remove(Character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
