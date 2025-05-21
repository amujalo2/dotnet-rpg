
using dotnet_rpg.Dtos.Weapon;
using System.Security.Claims;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService(DataContext context, IHttpContextAccessor accessor, IMapper mapper) : IWeaponService
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;
        public IHttpContextAccessor _accessor = accessor;

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User!.Id == int.Parse(_accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }
                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };
                _context.Weapons.Add(weapon);
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
