using Project.Core.Dto;
using Project.Core.Data.Model;

namespace Project.Core.Services.Config
{
    public class ModelToDtoCoreBase : AutoMapper.Profile
    {

        public ModelToDtoCoreBase()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDtoSave>().ReverseMap();
            CreateMap<Usuario, UsuarioDtoResult>().ReverseMap();
            CreateMap<Usuario, UsuarioDtoDetail>().ReverseMap();

        }

    }
}
