using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.TotalizerRole
{
    public class BaseTestTotalizerRole : BaseIntegration
    {
        protected TotalizerRoleRequestDto TotalizerRoleRequestDto;
        protected PageParams PageParams;

        protected BaseTestTotalizerRole()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };
            
            TotalizerRoleRequestDto = new TotalizerRoleRequestDto
            {
                Code = "",
                Type = (int)TotalizerType.Discriminated
            };
        }

        protected void GenerateRequestDto()
        {
            TotalizerRoleRequestDto = new TotalizerRoleRequestDto
            {
                Code = "HOM_REV",
                Type = (int)TotalizerType.Discriminated
            };
        }
    }
}