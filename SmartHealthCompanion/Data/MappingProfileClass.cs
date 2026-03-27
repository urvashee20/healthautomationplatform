using SmartHealthCompanion.DTOs;
using SmartHealthCompanion.Entities;
using System.Collections.Generic;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Data
{
    public class MappingProfileClass : AutoMapper.Profile
    {
        public MappingProfileClass()
        {
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<Goal, CreateGoalDto>()
                .ForMember(dest => dest.SecondaryGoals, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.SecondaryGoals) 
                    ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(src.SecondaryGoals, (System.Text.Json.JsonSerializerOptions)null) 
                    : null))
                .ForMember(dest => dest.HealthConditions, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.HealthConditions) 
                    ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(src.HealthConditions, (System.Text.Json.JsonSerializerOptions)null) 
                    : null));
            CreateMap<AIPlan, AIPlanDto>();
        }
    }
}
