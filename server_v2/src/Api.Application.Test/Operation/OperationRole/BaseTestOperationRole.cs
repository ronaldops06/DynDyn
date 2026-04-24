using Api.Domain.Dtos.Operation;
using Api.Domain.Models;
using Api.Application.V1.Controllers;
using Domain.Helpers;

namespace Api.Application.Test.Operation
{
    public class BaseTestOperationRole : BaseTestApplication
    {
        protected OperationRoleController Controller;
        protected OperationRoleModel operationRoleModel;
        protected OperationRoleRequestDto operationRoleRequestDto;
        protected PageParams PageParams;
        protected List<OperationRoleModel> ListTransactionRuleModel = new List<OperationRoleModel>();

        protected BaseTestOperationRole()
        {
            operationRoleModel = new OperationRoleModel
            {
                Id = 1,
                Name = "INSS",
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTransactionRuleModel.Add(operationRoleModel);

            operationRoleModel = new OperationRoleModel
            {
                Id = 2,
                Name = "Geral",
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTransactionRuleModel.Add(operationRoleModel);

            operationRoleModel = new OperationRoleModel
            {
                Id = 3,
                Name = "Salário",
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTransactionRuleModel.Add(operationRoleModel);
            
            operationRoleRequestDto = new OperationRoleRequestDto
            {
                Id = operationRoleModel.Id,
                Name = operationRoleModel.Name
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}