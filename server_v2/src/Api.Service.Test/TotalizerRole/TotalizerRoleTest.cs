using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test
{
    public class TotalizerRoleTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<ITotalizerRoleRepository> RepositoryMock = new Mock<ITotalizerRoleRepository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<TotalizerRoleModel> listTotalizerRoleModel = new List<TotalizerRoleModel>();
        protected List<TotalizerRoleModel> listTotalizerRoleModelResult = new List<TotalizerRoleModel>();
        protected TotalizerRoleModel totalizerRoleModel;
        protected TotalizerRoleModel totalizerRoleModelResult;
        protected TotalizerRoleModel totalizerRoleModelUpdate;
        protected TotalizerRoleModel totalizerRoleModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

        protected TotalizerRoleTest()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new TotalizerRoleModel()
                {
                    Id = i,
                    //
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listTotalizerRoleModel.Add(model);
            }

            listTotalizerRoleModelResult = listTotalizerRoleModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            totalizerRoleModel = new TotalizerRoleModel
            {
                Id = 2,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            totalizerRoleModelResult = new TotalizerRoleModel
            {
                Id = totalizerRoleModel.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            totalizerRoleModelUpdate = new TotalizerRoleModel
            {
                Id = totalizerRoleModel.Id,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            totalizerRoleModelUpdateResult = new TotalizerRoleModel
            {
                Id = totalizerRoleModelUpdate.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "totalizerRole",
                ReferenceId = totalizerRoleModel.Id
            };
        }

        protected void ApplyTest(TotalizerRoleModel totalizerRoleModelSource, TotalizerRoleModel totalizerRoleModelDest)
        {
            Assert.NotNull(totalizerRoleModelDest);
            Assert.NotNull(totalizerRoleModelDest.User);
            Assert.True(totalizerRoleModelDest.UserId > 0);
            Assert.Equal(totalizerRoleModelSource.Id, totalizerRoleModelDest.Id);
            //
            Assert.Equal(totalizerRoleModelSource.UserId, totalizerRoleModelDest.UserId);
            Assert.Equal(totalizerRoleModelSource.User.Id, totalizerRoleModelDest.User.Id);
        }
    }
}