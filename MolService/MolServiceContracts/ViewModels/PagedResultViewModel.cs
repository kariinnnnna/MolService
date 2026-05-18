using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public List<T> Items { get; set; } = new();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
