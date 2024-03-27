using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class UserTest : BaseTestService
    {
        protected string AccessToken;
        protected Mock<ILoginService> LoginServiceMock = new Mock<ILoginService>();
        protected Mock<IUserRepository> RepositoryMock = new Mock<IUserRepository>();
        protected List<UserModel> listUserModel = new List<UserModel>();
        protected List<UserModel> listUserModelResult = new List<UserModel>();
        protected UserModel userModel;
        protected UserModel userModelResult;
        protected UserModel userModelUpdate;
        protected UserModel userModelUpdateResult;
        protected PageParams pageParams;

        protected UserTest()
        {
            AccessToken = "kkjk3jjj3hhh3hh5h3kjhkjhdha.jh3hjhdhdjhdjhj";

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= 10; i++)
            {
                var dto = new UserModel()
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    Role = Faker.Lorem.GetFirstWord(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listUserModel.Add(dto);
            }

            listUserModelResult = listUserModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                               .Take(pageParams.PageSize)
                                               .ToList();

            userModel = new UserModel
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Login = Faker.Internet.Email(),
                Role = Faker.Lorem.GetFirstWord()
            };

            userModelResult = new UserModel
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Login = userModel.Login,
                Role = userModel.Role,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            userModelUpdate = new UserModel
            {
                Id = userModel.Id,
                Name = Faker.Name.FullName(),
                Login = Faker.Internet.Email(),
                Role = Faker.Lorem.GetFirstWord()
            };

            userModelUpdateResult = new UserModel
            {
                Id = userModelUpdate.Id,
                Name = userModelUpdate.Name,
                Login = userModelUpdate.Login,
                Role = userModelUpdate.Role,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }

        protected void ApplyTest(UserModel userModelSource, UserModel userModelDest)
        {
            Assert.NotNull(userModelDest);
            Assert.Equal(userModelSource.Id, userModelDest.Id);
            Assert.Equal(userModelSource.Name, userModelDest.Name);
            Assert.Equal(userModelSource.Login, userModelDest.Login);
            Assert.Equal(userModelSource.Role, userModelDest.Role);
        }
    }
}
