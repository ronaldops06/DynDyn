using System;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
