using AutoMapper;
using CrossCutting.Mappings;

namespace Api.Application.Test
{
    public abstract class BaseTestApplication
    {
        public IMapper Mapper { get; set; }

        public BaseTestApplication()
        {
            Mapper = new AutoMapperFixture().GetMapper();
        }

        public static string GeneratePassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
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
