using System;

namespace Somnia.API.Models
{
    public class ModelBase
    {
        public int ID { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
