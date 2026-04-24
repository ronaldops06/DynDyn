using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.{{folder}}
{
    public class {{model}}Test : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<I{{model}}Repository> RepositoryMock = new Mock<I{{model}}Repository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<{{model}}Model> list{{model}}Model = new List<{{model}}Model>();
        protected List<{{model}}Model> list{{model}}ModelResult = new List<{{model}}Model>();
        protected {{model}}Model {{alias}}Model;
        protected {{model}}Model {{alias}}ModelResult;
        protected {{model}}Model {{alias}}ModelUpdate;
        protected {{model}}Model {{alias}}ModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

        protected {{model}}Test()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new {{model}}Model()
                {
                    Id = i,
                    //
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                list{{model}}Model.Add(model);
            }

            list{{model}}ModelResult = list{{model}}Model.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            {{alias}}Model = new {{model}}Model
            {
                Id = 2,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            {{alias}}ModelResult = new {{model}}Model
            {
                Id = {{alias}}Model.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            {{alias}}ModelUpdate = new {{model}}Model
            {
                Id = {{alias}}Model.Id,
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            {{alias}}ModelUpdateResult = new {{model}}Model
            {
                Id = {{alias}}ModelUpdate.Id,
                //
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "{{alias}}",
                ReferenceId = {{alias}}Model.Id
            };
        }

        protected void ApplyTest({{model}}Model {{alias}}ModelSource, {{model}}Model {{alias}}ModelDest)
        {
            Assert.NotNull({{alias}}ModelDest);
            Assert.NotNull({{alias}}ModelDest.User);
            Assert.True({{alias}}ModelDest.UserId > 0);
            Assert.Equal({{alias}}ModelSource.Id, {{alias}}ModelDest.Id);
            //
            Assert.Equal({{alias}}ModelSource.UserId, {{alias}}ModelDest.UserId);
            Assert.Equal({{alias}}ModelSource.User.Id, {{alias}}ModelDest.User.Id);
        }
    }
}