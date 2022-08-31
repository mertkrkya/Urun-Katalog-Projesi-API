using System;
using System.Collections.Generic;

namespace UrunKatalogProjesi.Data.Dto
{
    public class PaginationResultDto<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int totalItems { get; set; }
        public int currentPage { get; set; }
    }
}
