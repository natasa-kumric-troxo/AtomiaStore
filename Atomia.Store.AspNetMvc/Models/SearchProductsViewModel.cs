﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public class SearchProductsViewModel
    {
        //[Required]
        public string SearchQuery { get; set; }
    }
}
