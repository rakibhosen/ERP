using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.ViewModels.Utility
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public Func<int, string> PageUrl { get; set; }
        public int MaxPagesToShow { get; set; } = 5; // Maximum number of pages to display in pagination
    }

}
