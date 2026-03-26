using SmartHealthCompanion.DTOs;
using SmartHealthCompanion.Entities;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Data
{
    public class MappingProfileClass : AutoMapper.Profile
    {
        public MappingProfileClass()
        {
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<Goal, CreateGoalDto>();
            CreateMap<AIPlan, AIPlanDto>();
        }
    }
}
