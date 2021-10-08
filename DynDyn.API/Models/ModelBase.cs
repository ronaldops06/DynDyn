using System;

namespace DynDyn.API.Models
{
    public class ModelBase
    {
        public int ID { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; }
    }
}
