using Project.Core.Dto;
using Project.Core.Data.Model;

namespace Project.Core.Services.Config
{
    public class ModelToDtoCoreBase : AutoMapper.Profile
    {

        public ModelToDtoCoreBase()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDtoSave>().ReverseMap();
            CreateMap<Categoria, CategoriaDtoResult>().ReverseMap();
            CreateMap<Categoria, CategoriaDtoDetail>().ReverseMap();

        }

    }
}
