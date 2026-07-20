using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Attribute
{
    public class AttributeCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public AttributeCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(AttributeEntity attributeEntitySource, AttributeEntity attributeEntityDest)
        {
            Assert.NotNull(attributeEntityDest);
            Assert.Equal(attributeEntitySource.Name, attributeEntityDest.Name);
            Assert.Equal(attributeEntitySource.Description, attributeEntityDest.Description);
            Assert.Equal(attributeEntitySource.DataType, attributeEntityDest.DataType);
            Assert.Equal(attributeEntitySource.Status, attributeEntityDest.Status);
            Assert.Equal(attributeEntitySource.UserId, attributeEntityDest.UserId);
            Assert.Equal(attributeEntitySource.User.Id, attributeEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de Atributo")]
        [Trait("CRUD", "AttributeEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Operacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referências

                AttributeRepository _repositorio = new AttributeRepository(context);

                AttributeEntity attributeEntity = new AttributeEntity()
                {
                    Id = 1,
                    Name = Faker.Name.FullName(),
                    Description = Faker.Lorem.Sentence(),
                    Status = StatusType.Ativo,
                    DataType = AttributeDataType.Number,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(attributeEntity);
                AplicaTesteCampos(attributeEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                attributeEntity.Status = StatusType.Inativo;
                attributeEntity.DataType = AttributeDataType.Text;

                var _registroAtualizado = await _repositorio.UpdateAsync(attributeEntity);
                AplicaTesteCampos(attributeEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(attributeEntity, _registroSelecionado);

                var _todosRegistros = await _repositorio.SelectAsync(userCreated.Id);
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroCriado.Id);
                Assert.True(_removeu);

                _registroCriado.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_registroCriado));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_registroCriado.Id));
            }
        }
    }
}