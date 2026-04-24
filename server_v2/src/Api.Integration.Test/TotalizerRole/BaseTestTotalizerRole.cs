using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.TotalizerRole
{
    public class BaseTestTotalizerRole : BaseIntegration
    {
        protected class TotalizerRoleBase
        {
            public int TotalizerRoleId { get; set; }
            //
        }

        protected TotalizerRoleRequestDto TotalizerRoleRequestDto;
        protected TotalizerRoleBase TotalizerRoleBaseDto;
        protected PageParams PageParams;

        protected BaseTestTotalizerRole()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };

            TotalizerRoleBaseDto = new TotalizerRoleBase
            {
                TotalizerRoleId = 2,
                //
            };

            TotalizerRoleRequestDto = new TotalizerRoleRequestDto
            {
                //                
            };
        }

        protected void GenerateRequestDto()
        {
            TotalizerRoleRequestDto = new TotalizerRoleRequestDto
            {
                Id = TotalizerRoleBaseDto.TotalizerRoleId,
                //
            };
        }
    }
}