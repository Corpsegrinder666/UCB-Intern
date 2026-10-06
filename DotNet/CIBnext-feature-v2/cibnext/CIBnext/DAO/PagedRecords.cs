using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class PagedRecords
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<object> Records { get; set; }
    }
}