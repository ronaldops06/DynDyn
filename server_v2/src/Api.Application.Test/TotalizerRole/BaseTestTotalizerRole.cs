using Api.Application.V1.Controllers;
using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test
{
    public class BaseTestTotalizerRole : BaseTestApplication
    {
        protected TotalizerRoleController Controller;
        protected TotalizerRoleModel TotalizerRoleModel;
        protected TotalizerRoleRequestDto TotalizerRoleRequestDto;
        protected PageParams PageParams;
        protected List<TotalizerRoleModel> ListTotalizerRoleModel = new List<TotalizerRoleModel>();

        protected BaseTestTotalizerRole()
        {
            //Referências

            TotalizerRoleModel = new TotalizerRoleModel
            {
                Id = 1,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTotalizerRoleModel.Add(TotalizerRoleModel);

            TotalizerRoleModel = new TotalizerRoleModel
            {
                Id = 2,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTotalizerRoleModel.Add(TotalizerRoleModel);

            TotalizerRoleModel = new TotalizerRoleModel
            {
                Id = 3,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListTotalizerRoleModel.Add(TotalizerRoleModel);

            //Referências

            TotalizerRoleRequestDto = new TotalizerRoleRequestDto
            {
                //
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}