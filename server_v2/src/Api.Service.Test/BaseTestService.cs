using Api.CrossCutting.Mappings;
using AutoMapper;
using CrossCutting.Mappings;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Moq;
using static Api.Service.Test.Helpers.BaseHelper;

namespace Api.Service.Test
{
    public abstract class BaseTestService
    {
        protected Mock<IUserService> UserServiceMock = new Mock<IUserService>();
        protected UserModel UserModelFake;
        public IMapper Mapper { get; set; }

        public BaseTestService()
        {
            UserModelFake = GetLoggedUserFake();
            UserServiceMock.Setup(m => m.GetLoggedUser()).ReturnsAsync(UserModelFake);
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
                cfg.AddProfile(new DictionaryToModelProfile());
                cfg.AddProfile(new ModelToModelProfile());
            });

            return config.CreateMapper();
        }

        public void Dispose()
        {

        }
    }
}
