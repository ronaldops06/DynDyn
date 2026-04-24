using Api.Domain.Dtos.Operation;
using Domain.Helpers;

namespace Api.Integration.Test.Operation
{
    public class BaseTestOperationRole : BaseIntegration
    {
        protected class OperationRoleBase
        {
            public int OperationRoleId { get; set; }
            public string OperationRoleName { get; set; }
        }

        protected OperationRoleRequestDto operationRoleRequestDto;
        protected OperationRoleBase operationRoleBaseDto;
        protected PageParams PageParams;

        protected BaseTestOperationRole()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };

            operationRoleBaseDto = new OperationRoleBase
            {
                OperationRoleId = 2,
                OperationRoleName = "Carro"
            };

            operationRoleRequestDto = new OperationRoleRequestDto
            {
                Name = ""
            };
        }

        protected void GenerateRequestDto()
        {
            operationRoleRequestDto = new OperationRoleRequestDto
            {
                Id = operationRoleBaseDto.OperationRoleId,
                Name = operationRoleBaseDto.OperationRoleName
            };
        }
    }
}