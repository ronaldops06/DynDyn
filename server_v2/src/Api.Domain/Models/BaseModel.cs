using System;

namespace Domain.Models
{
    public class BaseModel
    {
        public long Id { get; set; }

        private DateTime _dataCriacao;

        public DateTime DataCriacao
        {
            get { return _dataCriacao; }
            set
            {
                _dataCriacao = value == null ? DateTime.UtcNow : value;
            }
        }

        private DateTime _dataAlteracao;

        public DateTime DataAlteracao
        {
            get { return _dataAlteracao; }
            set { _dataAlteracao = value; }
        }
    }
}
