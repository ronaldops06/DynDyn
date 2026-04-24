using Api.Domain.Dtos.{{model}};
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.{{model}}
{
    public class BaseTest{{model}} : BaseIntegration
    {
        protected class {{model}}Base
        {
            public int {{model}}Id { get; set; }
            //
        }

        protected {{model}}RequestDto {{model}}RequestDto;
        protected {{model}}Base {{model}}BaseDto;
        protected PageParams PageParams;

        protected BaseTest{{model}}()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };

            {{model}}BaseDto = new {{model}}Base
            {
                {{model}}Id = 2,
                //
            };

            {{model}}RequestDto = new {{model}}RequestDto
            {
                //                
            };
        }

        protected void GenerateRequestDto()
        {
            {{model}}RequestDto = new {{model}}RequestDto
            {
                Id = {{model}}BaseDto.{{model}}Id,
                //
            };
        }
    }
}