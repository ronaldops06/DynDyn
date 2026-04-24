using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de {{name}}.
    /// </summary>
    public interface I{{model}}Repository : IRepository<{{model}}Entity>
    {
        /// <summary>
        /// Método responsável por retornar a {{name}} com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <returns>Entidade de {{name}} <see cref="{{model}}Entity"/>.</returns>
        Task<{{model}}Entity> SelectByUkAsync(int userId);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="{{model}}Entity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParent{{model}}({{model}}Entity {{model}}Entity);
    }
}