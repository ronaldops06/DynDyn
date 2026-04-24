using Api.Application.V1.Controllers;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.{{folder}}
{
    public class BaseTest{{model}} : BaseTestApplication
    {
        protected {{model}}Controller Controller;
        protected {{model}}Model {{model}}Model;
        protected {{model}}RequestDto {{model}}RequestDto;
        protected PageParams PageParams;
        protected List<{{model}}Model> List{{model}}Model = new List<{{model}}Model>();

        protected BaseTest{{model}}()
        {
            //Referências

            {{model}}Model = new {{model}}Model
            {
                Id = 1,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            List{{model}}Model.Add({{model}}Model);

            {{model}}Model = new {{model}}Model
            {
                Id = 2,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            List{{model}}Model.Add({{model}}Model);

            {{model}}Model = new {{model}}Model
            {
                Id = 3,
                //
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            List{{model}}Model.Add({{model}}Model);

            //Referências

            {{model}}RequestDto = new {{model}}RequestDto
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