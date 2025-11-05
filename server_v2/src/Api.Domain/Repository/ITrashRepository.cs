using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repository
{
    public interface ITrashRepository : IRepository<TrashEntity>
    {
        /// <summary>
        /// Método responsável por retornar a lixeira com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="reference">Entidade a ser excluída.</param>
        /// <param name="referenceId">Identificador do registro a ser excluído.</param>
        /// <returns>Entidade de lixeira. <see cref="TrashEntity"/></returns>
        Task<TrashEntity> SelectByUkAsync(int userId, string reference, int referenceId);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="trashEntity">Entidade a ter os dependetes com status alterado.</param>
        public void UnchangedParentTrash(TrashEntity trashEntity);
    }
}