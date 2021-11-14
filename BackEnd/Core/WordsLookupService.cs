using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Api.Dal;
using TestApp.Api.Dal.Models;

namespace TestApp.Api.Core
{
    public class WordsLookupService : IWordsLookupService
    {
        private readonly TestAppDbContext testAppDbContext;

        public WordsLookupService(TestAppDbContext testAppDbContext)
        {
            this.testAppDbContext = testAppDbContext;
        }

        public async Task<IList<LookupWord>> LookupWeighted(string searchString, int returnTopRecordsCount)
        {
            var result = new List<LookupWord>();

            try
            {
                if (!string.IsNullOrWhiteSpace(searchString) && returnTopRecordsCount > 0)
                {
                    var foundWords = testAppDbContext.SearchStrings.Where(x => x.String == searchString).OrderByDescending(x => x.Weight).Take(returnTopRecordsCount);

                    result = await foundWords.Select(x => x.LookupWord).ToListAsync();
                }                
                else
                {
                    result.Clear();
                    result.Add(new LookupWord { Id = 0, Word = $"Error: wrong criteria passed" });
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new LookupWord { Id = 0, Word = $"Error: {ex.Message}"});
            }

            return result;
        }

        public async Task<IList<LookupWord>> LookupStartMatchAlphabetical(string searchString, int returnTopRecordsCount)
        {
            var result = new List<LookupWord>();

            try
            {
                if (!string.IsNullOrWhiteSpace(searchString) && returnTopRecordsCount > 0)
                {
                    var lookupWeighted = await LookupWeighted(searchString, returnTopRecordsCount);

                    var startMatchAlpha = (from w in testAppDbContext.LookupWords
                                            where EF.Functions.Like(w.Word, $"{searchString}%")
                                            select w).OrderBy(x => x.Word);

                    // excluded weighted lookup results
                    var excludedWeighted = startMatchAlpha.Where(x => !lookupWeighted.Select(y => y.Id).Contains(x.Id));
                    result = await excludedWeighted.Take(returnTopRecordsCount).ToListAsync();
                }
                else
                {
                    result.Clear();
                    result.Add(new LookupWord { Id = 0, Word = $"Error: wrong criteria passed" });
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new LookupWord { Id = 0, Word = $"Error: {ex.Message}" });
            }

            return result;
        }

        public async Task<IList<LookupWord>> LookupContainingMatchAlphabetical(string searchString, int returnTopRecordsCount)
        {
            var result = new List<LookupWord>();

            try
            {
                if (!string.IsNullOrWhiteSpace(searchString) && returnTopRecordsCount > 0)
                {
                    var lookupWeighted = await LookupWeighted(searchString, returnTopRecordsCount);
                    var startMatchAlpha = await LookupStartMatchAlphabetical(searchString, returnTopRecordsCount);

                    var containingMatchAlpha = (from w in testAppDbContext.LookupWords
                                            where EF.Functions.Like(w.Word, $"%{searchString}%")
                                            select w).OrderBy(x => x.Word);

                    // excluded weighted lookup results
                    var excludeWeighted = containingMatchAlpha.Where(x => !lookupWeighted.Select(y => y.Id).Contains(x.Id));
                    // excluded start match alphabetical results
                    var excludeStartMatchAlpha = excludeWeighted.Where(x => !startMatchAlpha.Select(y => y.Id).Contains(x.Id));

                    result = await excludeStartMatchAlpha.Take(returnTopRecordsCount).ToListAsync();
                }
                else
                {
                    result.Clear();
                    result.Add(new LookupWord { Id = 0, Word = $"Error: wrong criteria passed" });
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new LookupWord { Id = 0, Word = $"Error: {ex.Message}" });
            }

            return result;
        }

        public async Task<bool> SelectWord(string searchString, int lookupWordId)
        {
            var result = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    var searchStringRecord = testAppDbContext.SearchStrings.SingleOrDefault(x => x.String == searchString && x.LookupWordId == lookupWordId);

                    if (searchStringRecord == null)
                    {
                        var newSearchRecord = new SearchString
                        {
                            LookupWordId = lookupWordId,
                            String = searchString.ToLower(),
                            Weight = 1
                        };

                        testAppDbContext.SearchStrings.Add(newSearchRecord);
                    }
                    else
                    {
                        searchStringRecord.Weight += 1;

                        testAppDbContext.SearchStrings.Update(searchStringRecord);
                    }

                    await testAppDbContext.SaveChangesAsync();

                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
