using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Api.Models
{
    public class SelectWordModel
    {
        public string SearchString { get; set; }
        public int LookupWordId { get; set; }
    }
}
