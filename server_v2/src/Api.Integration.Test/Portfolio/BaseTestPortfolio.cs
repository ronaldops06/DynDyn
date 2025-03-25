using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Portfolio
{
    public class BaseTestPortfolio : BaseIntegration
    {
        protected class CategoryBase
        {
            public int CategoryId { get; set; }
            public string CategoryNome { get; set; }
            public int CategoryTipo { get; set; }
            public int CategoryStatus { get; set; }
        }

        protected class Base
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public PortfolioType Type { get; set; }
            public PortfolioGroupType Group { get; set; }
            public int Status { get; set; }
            public CategoryBase Category { get; set; }
            public Base ParentPortfolio { get; set; }
        }

        protected PortfolioRequestDto PortfolioRequestDto;
        protected CategoryRequestDto CategoryRequestDto;
        protected PortfolioRequestDto ParentPortfolioRequestDto;
        protected Base PortfolioBaseDto;
        protected PageParams PageParams;

        protected BaseTestPortfolio()
        {
            PageParams = new PageParams()
            {
                Tipo = 1,
                PageNumber = 1,
                PageSize = 3
            };

            var categoryBase = new CategoryBase
            {
                CategoryId = 2,
                CategoryNome = "Corrente",
                CategoryTipo = (int)CategoryType.Conta,
                CategoryStatus = (int)StatusType.Ativo
            };

            var parentBase = new Base
            {
                Id = 1,
                Name = "Geral",
                Type = PortfolioType.Ativo,
                Group = PortfolioGroupType.ContasBancarias,
                Status = (int)StatusType.Ativo,
                Category = categoryBase
            };

            PortfolioBaseDto = new Base
            {
                Id = 2,
                Name = "Cash",
                Type = PortfolioType.Ativo,
                Group = PortfolioGroupType.ContasBancarias,
                Status = (int)StatusType.Ativo,
                Category = categoryBase,
                ParentPortfolio = parentBase
            };

            PortfolioRequestDto = new PortfolioRequestDto
            {
                Name = "",
                Type = PortfolioType.Ativo,
                Group = PortfolioGroupType.ContasBancarias,
                Status = 3,
                Category = null
            };
        }

        protected void GenerateRequestDto()
        {
            CategoryRequestDto = new CategoryRequestDto()
            {
                Id = PortfolioBaseDto.Category.CategoryId,
                Name = PortfolioBaseDto.Category.CategoryNome,
                Status = PortfolioBaseDto.Category.CategoryStatus,
                Type = PortfolioBaseDto.Category.CategoryTipo
            };

            ParentPortfolioRequestDto = new PortfolioRequestDto
            {
                Id = PortfolioBaseDto.ParentPortfolio.Id,
                Name = PortfolioBaseDto.ParentPortfolio.Name,
                Type = PortfolioBaseDto.Type,
                Group = PortfolioBaseDto.Group,
                Status = PortfolioBaseDto.ParentPortfolio.Status,
                Category = CategoryRequestDto
            };

            PortfolioRequestDto = new PortfolioRequestDto
            {
                Id = PortfolioBaseDto.Id,
                Name = PortfolioBaseDto.Name,
                Type = PortfolioBaseDto.Type,
                Group = PortfolioBaseDto.Group,
                Status = PortfolioBaseDto.Status,
                ParentPortfolio = ParentPortfolioRequestDto,
                Category = CategoryRequestDto
            };
        }
    }
}