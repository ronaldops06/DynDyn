using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Domain.Models;
using Faker;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Portfolio
{
    public class BaseTestPortfolio : BaseTestApplication
    {
        protected PortfolioController Controller;
        protected PortfolioModel PortfolioModel;
        protected PortfolioRequestDto PortfolioRequestDto;
        protected PageParams PageParams;
        protected List<PortfolioModel> ListPortfolioModel = new List<PortfolioModel>();

        protected BaseTestPortfolio()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo
            };

            var parentModel = new PortfolioModel
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel
            };

            PortfolioModel = new PortfolioModel
            {
                Id = 1,
                Name = Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentPortfolioId = parentModel.Id,
                ParentPortfolio = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListPortfolioModel.Add(PortfolioModel);

            PortfolioModel = new PortfolioModel
            {
                Id = 2,
                Name = Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentPortfolioId = parentModel.Id,
                ParentPortfolio = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListPortfolioModel.Add(PortfolioModel);

            PortfolioModel = new PortfolioModel
            {
                Id = 3,
                Name = Name.FullName(),
                Status = GetStatusTypeRandom(),
                ParentPortfolioId = parentModel.Id,
                ParentPortfolio = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListPortfolioModel.Add(PortfolioModel);

            var categoryRequestDto = new CategoryRequestDto
            {
                Id = 1
            };

            var parentPortfolioRequestDto = new PortfolioRequestDto
            {
                Id = 1
            };

            PortfolioRequestDto = new PortfolioRequestDto
            {
                Name = PortfolioModel.Name,
                Status = (int)PortfolioModel.Status,
                ParentPortfolio = parentPortfolioRequestDto,
                Category = categoryRequestDto,
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
    }
}