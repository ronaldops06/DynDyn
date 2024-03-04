using System;
using System.Collections.Generic;

namespace Domain.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList(List<T> itens, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(itens);
        }

        public static PageList<T> Create(PageParams pageParams, List<T> itens, int count)
        {
            return new PageList<T>(itens, count, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
