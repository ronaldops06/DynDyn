using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.OperationRole.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestOperationRole
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IOperationRoleService>();

            var pageListTransactionRuleModel = PageList<OperationRoleModel>.Create(PageParams, ListTransactionRuleModel, ListTransactionRuleModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListTransactionRuleModel);

            Controller = new OperationRoleController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<OperationRoleResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}