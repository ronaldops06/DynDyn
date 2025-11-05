using System.Threading.Tasks;
using Domain.Helpers;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    public interface ITrashService
    {
        /// <summary>
        /// Método responsável por retornar a(s) categoria(s) com base nos parâmetros informados.
        /// </summary>
        /// <param name="pageParams">Parâmetros para filtro <see cref="PageParams"/>.</param>
        /// <returns>Objeto de Lista com as informações das categorias encontradas.</returns>
        Task<PageList<TrashModel>> Get(PageParams pageParams);

        /// <summary>
        /// Método responsável por realizar a inclusão do registro.
        /// </summary>
        /// <param name="model">Modelo a ser inserido</param>
        /// <returns>Modelo inserido.</returns>
        Task<TrashModel> Post(TrashModel model);
    }
}