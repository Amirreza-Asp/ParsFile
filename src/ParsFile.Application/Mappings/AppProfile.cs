using AutoMapper;
using ParsFile.Domain.Dtos.Account;
using ParsFile.Domain.Dtos.Content.File;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Application.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<User, LoginRegisterDto>().ReverseMap();

            CreateMap<Domain.Entities.Content.File, AddFileDto>().ReverseMap();
            CreateMap<Domain.Entities.Content.File, FileDto>().ReverseMap();
            CreateMap<Domain.Entities.Content.File, FileSearchDto>().ReverseMap();
        }
    }
}
