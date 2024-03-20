using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Service.Test.Helpers.CategoryHelpers;

namespace Api.Service.Test.Category
{
    public class CategoryTest
    {
        public static string CategoryNome { get; set; }
        public static CategoryType CategoryTipo { get; set; }
        public static StatusType CategoryStatus { get; set; }
        public static string CategoryNomeUpdated { get; set; }
        public static StatusType CategoryStatusUpdated { get; set; }
        public static CategoryType CategoryTipoUpdated { get; set; }
        public static int CategoryId { get; set; }

        public List<CategoryModel> listCategoryModel = new List<CategoryModel>();
        public CategoryModel categoryModel;
        public CategoryModel categoryModelResult;
        public CategoryModel categoryModelUpdate;
        public CategoryModel categoryModelUpdateResult;
        public PageParams pageParams;
        public PageList<CategoryModel> pageListResult;

        public CategoryTest()
        {
            CategoryId = 1;
            CategoryNome = Faker.Name.FullName();
            CategoryTipo = GetCategoryTypeRandom();
            CategoryNomeUpdated = Faker.Name.FullName();
            CategoryTipoUpdated = GetCategoryTypeRandom();

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= 10; i++)
            {
                var dto = new CategoryModel()
                {
                    Id = i,
                    Nome = Faker.Name.FullName(),
                    Tipo = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listCategoryModel.Add(dto);
            }

            pageListResult = new PageList<CategoryModel>(listCategoryModel.Skip(0).Take(4).ToList(), 5, pageParams.PageNumber, pageParams.PageSize);

            categoryModel = new CategoryModel
            {
                Id = CategoryId,
                Nome = CategoryNome,
                Tipo = CategoryTipo,
                Status = CategoryStatus
            };

            categoryModelResult = new CategoryModel
            {
                Id = CategoryId,
                Nome = CategoryNome,
                Tipo = CategoryTipo,
                Status = CategoryStatus,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            categoryModelUpdate = new CategoryModel
            {
                Id = CategoryId,
                Nome = CategoryNomeUpdated,
                Tipo = CategoryTipoUpdated,
                Status = CategoryStatusUpdated
            };

            categoryModelUpdateResult = new CategoryModel
            {
                Id = CategoryId,
                Nome = CategoryNomeUpdated,
                Tipo = CategoryTipoUpdated,
                Status = CategoryStatusUpdated,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }
    }
}