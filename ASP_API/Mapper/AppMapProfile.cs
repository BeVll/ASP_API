using ASP_API.Data.Entities;
using ASP_API.Models;
using AutoMapper;
using ASP_API.Data.Entities;
using ASP_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP_API.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>()
                .ForMember(x => x.ParentName, opt => opt.MapFrom(x => x.Parent.Name));

            CreateMap<CategoryEntity, CategoryEditViewModel>()
                .ForMember(x => x.ParentId, opt => opt.MapFrom(x => x.Parent.Id));

            CreateMap<CategoryCreateViewModel, CategoryEntity>()
                .ForMember(x => x.ParentId, opt => opt.MapFrom(x => x.ParentId == 0 ? null : x.ParentId))
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}