using System.Collections.Generic;

namespace Domain.Models
{
    public class Data<T>
    {
        public int Count { get; set; }
        public List<T> Itens { get; set; }

        public Data(int count, List<T> itens)
        {
            Count = count;
            Itens = itens;
        }
    }
}
