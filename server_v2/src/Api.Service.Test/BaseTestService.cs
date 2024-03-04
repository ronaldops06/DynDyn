using AutoMapper;
using CrossCutting.Mappings;

namespace Api.Service.Test
{
    public abstract class BaseTestService
    {
        public IMapper Mapper { get; set; }

        public BaseTestService()
        {
            Mapper = new AutoMapperFixture().GetMapper();
        }
    }

    public class AutoMapperFixture : IDisposable
    {
        public IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoToModelProfile());
                cfg.AddProfile(new EntityToModelProfile());
            });

            return config.CreateMapper();
        }

        public void Dispose()
        {

        }
    }
}
