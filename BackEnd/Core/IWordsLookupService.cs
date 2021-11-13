using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Api.Dal.Models;

namespace TestApp.Api.Core
{
    public interface IWordsLookupService
    {
        Task<IList<LookupWord>> LookupWeighted(string searchString, int returnTopRecordsCount);
        Task<IList<LookupWord>> LookupStartMatchAlphabetical(string searchString, int returnTopRecordsCount);
        Task<IList<LookupWord>> LookupContainingMatchAlphabetical(string searchString, int returnTopRecordsCount);
        Task<bool> SelectWord(string searchString, int lookupWordId);
    }
}
