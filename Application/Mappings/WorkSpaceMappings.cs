using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings
{
    public class WorkSpaceMappings : Profile
    {
        public WorkSpaceMappings()
        {
            CreateMap<WorkSpace, CreateWorkspaceViewModel>()
                .ForMember(x => x.UserId, x => x.MapFrom(x => x.User!.Id));
        }
    }
}
