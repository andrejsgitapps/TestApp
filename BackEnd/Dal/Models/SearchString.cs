using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Api.Dal.Models
{
    public class SearchString
    {
        [Key]
        public int Id { get; set; }
        
        public int LookupWordId { get; set; }
        [ForeignKey("LookupWordId")]
        public LookupWord LookupWord { get; set; }

        public string String { get; set; }
        public int Weight { get; set; }
    }
}
