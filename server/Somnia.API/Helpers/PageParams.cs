using System;

namespace Somnia.API.Helpers
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public DateTime? DataCriacaoInicio { get; set; }
        public DateTime? DataCriacaoFim { get; set; }
        public int? Tipo { get; set; }
        public int? MovimentoPaiID { get; set; }
    }
}
