using System;

namespace Api.Domain.Dtos
{
    /// <summary>
    /// Objeto de transferência de dados base.
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Identificador do objeto.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Data de criação do objeto.
        /// </summary>
        public DateTime? DataCriacao { get; set; }
        
        /// <summary>
        /// Data de criação do objeto.
        /// </summary>
        public DateTime? DataAlteracao { get; set; }
    }
}