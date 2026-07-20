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
    public class AttributeTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<IAttributeRepository> RepositoryMock = new Mock<IAttributeRepository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<AttributeModel> listAttributeModel = new List<AttributeModel>();
        protected List<AttributeModel> listAttributeModelResult = new List<AttributeModel>();
        protected AttributeModel attributeModel;
        protected AttributeModel attributeModelResult;
        protected AttributeModel attributeModelUpdate;
        protected AttributeModel attributeModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

        protected AttributeTest()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new AttributeModel()
                {
                    Id = i,
                    //
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listAttributeModel.Add(model);
            }

            listAttributeModelResult = listAttributeModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            attributeModel = new AttributeModel
            {
                Id = 2,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            attributeModelResult = new AttributeModel
            {
                Id = attributeModel.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            attributeModelUpdate = new AttributeModel
            {
                Id = attributeModel.Id,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            attributeModelUpdateResult = new AttributeModel
            {
                Id = attributeModelUpdate.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "attribute",
                ReferenceId = attributeModel.Id
            };
        }

        protected void ApplyTest(AttributeModel attributeModelSource, AttributeModel attributeModelDest)
        {
            Assert.NotNull(attributeModelDest);
            Assert.NotNull(attributeModelDest.User);
            Assert.True(attributeModelDest.UserId > 0);
            Assert.Equal(attributeModelSource.Id, attributeModelDest.Id);
            //
            Assert.Equal(attributeModelSource.UserId, attributeModelDest.UserId);
            Assert.Equal(attributeModelSource.User.Id, attributeModelDest.User.Id);
        }
    }
}