using Data.Context;
using Data.Repository;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.User
{
    public class UserExecuteGetAll : BaseTestGet<UserEntity>, IClassFixture<DbTest>
    {
        public UserExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Usuário")]
        [Trait("GET", "UserEntity")]
        public async Task Eh_Possivel_Realizar_Get_Usuario()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                UserRepository _repositorio = new UserRepository(context);
                List<UserEntity> listUserDto = new List<UserEntity>();

                for (int i = 1; i < 35; i++)
                {
                    UserEntity _entity = new UserEntity
                    {
                        Login = Faker.Internet.Email(),
                        Name = Faker.Name.FullName(),
                        Password = Faker.Lorem.GetFirstWord(),
                        Role = ""
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                base.RealizaGetPaginado(_repositorio);
            }
        }
    }
}
