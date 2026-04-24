using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.TotalizerRole
{
    public class TotalizerRoleCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public TotalizerRoleCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(TotalizerRoleEntity TotalizerRoleEntitySource, TotalizerRoleEntity TotalizerRoleEntityDest)
        {
            Assert.NotNull(TotalizerRoleEntityDest);
            Assert.Equal(TotalizerRoleEntitySource.Code, TotalizerRoleEntityDest.Code);
            Assert.Equal(TotalizerRoleEntitySource.Type, TotalizerRoleEntityDest.Type);
            Assert.Equal(TotalizerRoleEntitySource.UserId, TotalizerRoleEntityDest.UserId);
            Assert.Equal(TotalizerRoleEntitySource.User.Id, TotalizerRoleEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de papel de totalizador")]
        [Trait("CRUD", "TotalizerRoleEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Operacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referências

                TotalizerRoleRepository _repositorio = new TotalizerRoleRepository(context);

                TotalizerRoleEntity _TotalizerRoleEntity = new TotalizerRoleEntity()
                {
                    
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(_TotalizerRoleEntity);
                AplicaTesteCampos(_TotalizerRoleEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                //Adicionar campos para update

                var _registroAtualizado = await _repositorio.UpdateAsync(_TotalizerRoleEntity);
                AplicaTesteCampos(_TotalizerRoleEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(_TotalizerRoleEntity, _registroSelecionado);

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