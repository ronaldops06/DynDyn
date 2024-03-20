using System;

namespace Domain.Models
{
    /// <summary>
    /// Objeto de modelo base.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        public int Id { get; set; }

        private DateTime _dataCriacao;

        /// <summary>
        /// Data de criação do registro.
        /// </summary>
        public DateTime DataCriacao
        {
            get { return _dataCriacao; }
            set
            {
                _dataCriacao = value == null ? DateTime.UtcNow : value;
            }
        }

        private DateTime _dataAlteracao;

        /// <summary>
        /// Data de atualização do registro.
        /// </summary>
        public DateTime DataAlteracao
        {
            get { return _dataAlteracao; }
            set { _dataAlteracao = value; }
        }
    }
}
