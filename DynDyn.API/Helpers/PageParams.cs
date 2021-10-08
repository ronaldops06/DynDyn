﻿namespace DynDyn.API.Helpers
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
    }
}
