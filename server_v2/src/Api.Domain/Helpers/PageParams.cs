using System;

namespace Domain.Helpers
{
    public class PageParams
    {
        public const int MaxPageSize = 200;
        private int pageSize = MaxPageSize;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public DateTime? DataCriacaoInicio { get; set; }
        public DateTime? DataCriacaoFim { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public DateTime? BaseDate { get; set; }
        public int? Tipo { get; set; }
        public int? MovimentoPaiID { get; set; }
    }
}
