using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(x => x.RefreshToken, x => x.AllowNull())
                .ForMember(x => x.RefreashTokenExpirationTime, x => x.MapFrom(x => GenerateExpirationTime()))
                .ForMember(x => x.PasswordHash, x => x.AllowNull());

            CreateMap<User, UserInfoViewModel>()
                .ForMember(x => x.TokenJWT, x => x.AllowNull());
        }

        private DateTime GenerateExpirationTime()
        {
            return DateTime.Now.AddDays(2);
        }
    }
}
