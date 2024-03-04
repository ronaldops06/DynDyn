using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test
{
    public class UserCrudComplete : BaseTest, IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public UserCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "CRUD de Usuário")]
        [Trait("CRUD", "UserEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Usuario()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository _repositorio = new UserRepository(context);
                UserEntity _entity = new UserEntity
                {
                    Login = Faker.Internet.Email(),
                    Name = Faker.Name.FullName(),
                    Password = Faker.Lorem.GetFirstWord(),
                    Role = ""
                };

                var _registroCriado = await _repositorio.InsertAsync(_entity);
                Assert.NotNull(_registroCriado);
                Assert.True(_registroCriado.Id > 0);
                Assert.Equal(_entity.Login, _registroCriado.Login);
                Assert.Equal(_entity.Name, _registroCriado.Name);
                Assert.Equal(_entity.Password, _registroCriado.Password);
                Assert.Equal(_entity.Role, _registroCriado.Role);

                _entity.Name = Faker.Name.First();
                _entity.Password = Faker.Lorem.GetFirstWord();
                var _registroAtualizado = await _repositorio.UpdateAsync(_entity);
                Assert.NotNull(_registroAtualizado);
                Assert.Equal(_registroCriado.Id, _registroAtualizado.Id);
                Assert.Equal(_entity.Login, _registroAtualizado.Login);
                Assert.Equal(_entity.Name, _registroAtualizado.Name);
                Assert.Equal(_entity.Password, _registroAtualizado.Password);
                Assert.Equal(_entity.Role, _registroAtualizado.Role);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectAsync(_registroAtualizado.Id);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroAtualizado.Login, _registroSelecionado.Login);
                Assert.Equal(_registroAtualizado.Name, _registroSelecionado.Name);
                Assert.Equal(_registroAtualizado.Password, _registroSelecionado.Password);
                Assert.Equal(_registroAtualizado.Role, _registroSelecionado.Role);

                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroSelecionado.Id);
                Assert.True(_removeu);

                _entity.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_entity));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_entity.Id));

                var _usuarioPadrao = await _repositorio.FindUsuarioByLogin("admin@gmail.com");
                Assert.NotNull(_usuarioPadrao);
                Assert.Equal("admin@gmail.com", _usuarioPadrao.Login);
                Assert.Equal("Administrador", _usuarioPadrao.Name);
                Assert.Equal("pgadmin", _usuarioPadrao.Password);

                _usuarioPadrao = await _repositorio.SelectAsync(1);
                Assert.NotNull(_usuarioPadrao);
                Assert.Equal("admin@gmail.com", _usuarioPadrao.Login);
                Assert.Equal("Administrador", _usuarioPadrao.Name);
                Assert.Equal("pgadmin", _usuarioPadrao.Password);

                _usuarioPadrao = await _repositorio.FindUsuarioByUsernamaAndPassword("admin@gmail.com", "pgadmin");
                Assert.NotNull(_usuarioPadrao);
                Assert.Equal("admin@gmail.com", _usuarioPadrao.Login);
                Assert.Equal("Administrador", _usuarioPadrao.Name);
                Assert.Equal("pgadmin", _usuarioPadrao.Password);
            }
        }
    }
}
