using System.Collections.Generic;
using WebAPICoreDapper.Models;

namespace DotNetDapper.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRow { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
