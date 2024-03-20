
using Data.Repository;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test
{
    public class BaseTestGet<T> where T : BaseEntity
    {
        protected ServiceProvider serviceProvider;

        public BaseTestGet(DbTest dbTest)
        {
            serviceProvider = dbTest.ServiceProvider;
        }

        protected async Task RealizaGetPaginado(BaseRepository<T> repositorio)
        {
            PageParams pageParams = new PageParams
            {
                PageSize = 10,
                PageNumber = 1
            };

            Data<T> data = await repositorio.SelectByParamAsync(pageParams);
            Assert.NotNull(data);
            Assert.Equal(10, data.Itens.Count);

            pageParams = new PageParams
            {
                PageSize = 10,
                PageNumber = 2
            };

            data = await repositorio.SelectByParamAsync(pageParams);
            Assert.NotNull(data);
            Assert.Equal(10, data.Itens.Count);
            Assert.Equal(11, data.Itens.First().Id);
            Assert.Equal(20, data.Itens.Last().Id);
            Assert.Equal(11, data.Itens.Min(x => x.Id));
            Assert.Equal(20, data.Itens.Max(x => x.Id));

            pageParams = new PageParams
            {
                PageSize = 20,
                PageNumber = 2
            };

            data = await repositorio.SelectByParamAsync(pageParams);
            Assert.NotNull(data);
            Assert.Equal(15, data.Itens.Count);
            Assert.Equal(21, data.Itens.First().Id);
            Assert.Equal(35, data.Itens.Last().Id);
            Assert.Equal(21, data.Itens.Min(x => x.Id));
            Assert.Equal(35, data.Itens.Max(x => x.Id));
        }
    }
}
