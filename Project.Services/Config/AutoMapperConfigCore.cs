using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Core.Services.Config
{
	public class AutoMapperConfigCore
    {
		public static void RegisterMappings(IServiceCollection services)
		{
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new ModelToDtoCoreBase());
				mc.AddProfile(new ModelToDtoCore());
			});

			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);

		}
	}
}
