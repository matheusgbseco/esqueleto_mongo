using Common.Domain.Enums;
using System;

namespace Common.Domain.Base
{
    public class FilterBase
    {
        public FilterBase()
        {
            this.PageIndex = 0;
            this.PageSize = 50;
            this.IsPagination = true;
        }

        public int PageSkipped
        {
            get
            {
                return (this.PageIndex > 0 ? this.PageIndex - 1 : 0) * this.PageSize;
            }
        }

        public string AttributeBehavior { get; set; }

        public string[] OrderFields { get; set; }
        public OrderByType OrderByType { get; set; }

        public bool IsPagination { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        
    }
}
