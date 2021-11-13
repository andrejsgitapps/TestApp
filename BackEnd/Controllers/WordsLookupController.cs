using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Api.Core;
using TestApp.Api.Models;

namespace TestApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = 0)]
    public class WordsLookupController : ControllerBase
    {
        private readonly IWordsLookupService wordsLookupService;

        public WordsLookupController(IWordsLookupService wordsLookupService)
        {
            this.wordsLookupService = wordsLookupService;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IList<LookupWordModel>> LookupWeighted([FromBody] LookupModel model)
        {            
            var weightedLookupResults = await wordsLookupService.LookupWeighted(model.SearchString, model.ReturnTopRecordsCount);            
            return weightedLookupResults.Select(x => new LookupWordModel { Id = x.Id, Word = x.Word }).ToList();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IList<LookupWordModel>> LookupStartMatchAlpha([FromBody] LookupModel model)
        {
            var startMatchAlphaLookupResults = await wordsLookupService.LookupStartMatchAlphabetical(model.SearchString, model.ReturnTopRecordsCount);
            return startMatchAlphaLookupResults.Select(x => new LookupWordModel { Id = x.Id, Word = x.Word }).ToList();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IList<LookupWordModel>> LookupContainingMatchAlpha([FromBody] LookupModel model)
        {
            var containingMatchLookupResults = await wordsLookupService.LookupContainingMatchAlphabetical(model.SearchString, model.ReturnTopRecordsCount);
            return containingMatchLookupResults.Select(x => new LookupWordModel { Id = x.Id, Word = x.Word }).ToList();
        }

        public async Task<bool> SelectWord([FromBody] SelectWordModel model)
        {
            return await wordsLookupService.SelectWord(model.SearchString, model.LookupWordId);
        }
    }
}
