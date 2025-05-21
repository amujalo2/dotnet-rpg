using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController(IWeaponService weaponService) : ControllerBase
    {
        private readonly IWeaponService _weaponService = weaponService;

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = await _weaponService.AddWeapon(newWeapon);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
