using dotnet_rpg.Dtos.Skill;

namespace dotnet_rpg.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}
