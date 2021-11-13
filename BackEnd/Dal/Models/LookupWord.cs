using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Api.Dal.Models
{
    public class LookupWord
    {
        [Key]
        public int Id { get; set; }
        public string Word { get; set; }

        public virtual IList<SearchString> SearchStrings { get; set; }
    }
}
