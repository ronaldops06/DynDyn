using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.PendingUser;

public class TransientUserCrudComplete : IClassFixture<DbTest>
{
    private ServiceProvider _serviceProvider;

    public TransientUserCrudComplete(DbTest dbTest)
    {
        _serviceProvider = dbTest.ServiceProvider;
    }
    
    [Fact(DisplayName = "CRUD de Usuário Pendente")]
        [Trait("CRUD", "PendingUserEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Usuario()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                TransientUserRepository _repositorio = new TransientUserRepository(context);
                TransientUserEntity _entity = new TransientUserEntity
                {
                    Login = Faker.Internet.Email(),
                    Name = Faker.Name.FullName(),
                    Password = Faker.Lorem.GetFirstWord(),
                    VerificationCode = Faker.RandomNumber.Next(100000, 999999),
                    ExpirationDate = new DateTime().AddMinutes(15),
                };

                var _registroCriado = await _repositorio.InsertAsync(_entity);
                Assert.NotNull(_registroCriado);
                Assert.True(_registroCriado.Id > 0);
                Assert.Equal(_entity.Login, _registroCriado.Login);
                Assert.Equal(_entity.Name, _registroCriado.Name);
                Assert.Equal(_entity.Password, _registroCriado.Password);
                Assert.Equal(_entity.VerificationCode, _registroCriado.VerificationCode);
                Assert.Equal(_entity.ExpirationDate, _registroCriado.ExpirationDate);

                _entity.Name = Faker.Name.First();
                _entity.Password = Faker.Lorem.GetFirstWord();
                _entity.VerificationCode = Faker.RandomNumber.Next(100000, 999999);
                
                var _registroAtualizado = await _repositorio.UpdateAsync(_entity);
                Assert.NotNull(_registroAtualizado);
                Assert.Equal(_registroCriado.Id, _registroAtualizado.Id);
                Assert.Equal(_entity.Login, _registroAtualizado.Login);
                Assert.Equal(_entity.Name, _registroAtualizado.Name);
                Assert.Equal(_entity.Password, _registroAtualizado.Password);
                Assert.Equal(_entity.VerificationCode, _registroAtualizado.VerificationCode);
                Assert.Equal(_entity.ExpirationDate, _registroAtualizado.ExpirationDate);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(_registroAtualizado.Id);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroAtualizado.Login, _registroSelecionado.Login);
                Assert.Equal(_registroAtualizado.Name, _registroSelecionado.Name);
                Assert.Equal(_registroAtualizado.Password, _registroSelecionado.Password);
                Assert.Equal(_entity.VerificationCode, _registroAtualizado.VerificationCode);
                Assert.Equal(_entity.ExpirationDate, _registroAtualizado.ExpirationDate);
                
                _registroSelecionado = await _repositorio.SelectUsuarioByLogin(_registroAtualizado.Login);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroAtualizado.Login, _registroSelecionado.Login);
                Assert.Equal(_registroAtualizado.Name, _registroSelecionado.Name);
                Assert.Equal(_registroAtualizado.Password, _registroSelecionado.Password);
                Assert.Equal(_entity.VerificationCode, _registroAtualizado.VerificationCode);
                Assert.Equal(_entity.ExpirationDate, _registroAtualizado.ExpirationDate);
                
                var _removeu = await _repositorio.DeleteAsync(_registroSelecionado.Id);
                Assert.True(_removeu);

                _entity.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_entity));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_entity.Id));
            }
        }
}