using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helper
{
    public class QueryObject
    {
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int Skip { get; set; } = 1;
        public int Take { get; set; } = 20;
    }
}