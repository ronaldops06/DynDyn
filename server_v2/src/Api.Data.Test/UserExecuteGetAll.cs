using Data.Context;
using Data.Repository;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test
{
    public class UserExecuteGetAll : BaseTest, IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public UserExecuteGetAll(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "Get de Usuário")]
        [Trait("GET", "UserEntity")]
        public async Task Eh_Possivel_Realizar_Get_Usuario()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
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

                PageParams pageParams = new PageParams { 
                    PageSize = 10,
                    PageNumber = 1
                };

                Data<UserEntity> usuarios = await _repositorio.FindAllUsuariosAsync(pageParams);
                Assert.NotNull(usuarios);
                Assert.Equal(10, usuarios.Itens.Count);

                pageParams = new PageParams
                {
                    PageSize = 10,
                    PageNumber = 2
                };

                usuarios = await _repositorio.FindAllUsuariosAsync(pageParams);
                Assert.NotNull(usuarios);
                Assert.Equal(10, usuarios.Itens.Count);
                Assert.Equal(11, usuarios.Itens.First().Id);
                Assert.Equal(20, usuarios.Itens.Last().Id);
                Assert.Equal(11, usuarios.Itens.Min(x => x.Id));
                Assert.Equal(20, usuarios.Itens.Max(x => x.Id));                

                pageParams = new PageParams
                {
                    PageSize = 20,
                    PageNumber = 2
                };

                usuarios = await _repositorio.FindAllUsuariosAsync(pageParams);
                Assert.NotNull(usuarios);
                Assert.Equal(15, usuarios.Itens.Count);
                Assert.Equal(21, usuarios.Itens.First().Id);
                Assert.Equal(35, usuarios.Itens.Last().Id);
                Assert.Equal(21, usuarios.Itens.Min(x => x.Id));
                Assert.Equal(35, usuarios.Itens.Max(x => x.Id));
            }
        }
    }
}
