using Microsoft.EntityFrameworkCore;
using Moq;
using Somnia.API.Data;
using Somnia.API.Helpers;
using Somnia.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Somnia.UnitTest.Repo
{
    public class CategoriaRepositoryTest
    {

        [Fact]
        public async void FindAllCategories()
        {
            // Arrange
            /*IQueryable<Categoria> data = GenerateListCategory().AsQueryable();

            var mockSet = new Mock<DbSet<Categoria>>();
            mockSet.As<IAsyncEnumerable<Categoria>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(() => new TestAsyncEnumerator<Categoria>(data.GetEnumerator()));
            mockSet.As<IQueryable<Categoria>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Categoria>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Categoria>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Categoria>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SomniaContext>();
            mockContext.Setup(c => c.Categorias).Returns(mockSet.Object);

            // Act
            PageParams pageParams = new PageParams();
            var repository = new Repository(mockContext.Object);
            var result = await repository.GetAllCategoriasAsync(pageParams);

            // Assert
            Assert.Equal(5, result.Count);*/
        }

  
        private List<Categoria> GenerateListCategory()
        {
            List<Categoria> categorias = new List<Categoria>();

            categorias.Add(GenerateCategory(1));
            categorias.Add(GenerateCategory(2));
            categorias.Add(GenerateCategory(3));
            categorias.Add(GenerateCategory(4));
            categorias.Add(GenerateCategory(5));

            return categorias;
        }

        private Categoria GenerateCategory(int id)
        {
            Categoria categoria = new Categoria();
            categoria.ID = id;
            categoria.Nome = $"Categoria {id}";

            return categoria;
        }
    }
}
