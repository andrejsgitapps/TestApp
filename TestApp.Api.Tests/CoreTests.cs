using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestApp.Api.Core;
using TestApp.Api.Dal;
using TestApp.Api.Dal.Models;

namespace TestApp.Api.Tests
{
    public class CoreTests
    {
        private readonly TestAppDbContext dbContext;
        private readonly IWordsLookupService wordsLookupService;

        public CoreTests()
        {
            var options = new DbContextOptionsBuilder<TestAppDbContext>().UseInMemoryDatabase(databaseName: "TestAppDb").Options;
            dbContext = new TestAppDbContext(options);

            LoadTestData(dbContext);

            wordsLookupService = new WordsLookupService(dbContext);
        }

        [Test]
        public void LookupWeighted_Mic_TenMicrophoneFiveMicroscope_ReturnsMicrophoneMicroscope()
        {
            var result = wordsLookupService.LookupWeighted("mic", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Microphone", result[0].Word);
            Assert.AreEqual("Microscope", result[1].Word);
        }

        [Test]
        public void LookupWeighted_Micr_ElevenMicroscopeOneMicrosoft_ReturnsMicroscopeMicrosoft()
        {
            var result = wordsLookupService.LookupWeighted("micr", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Microscope", result[0].Word);
            Assert.AreEqual("Microsoft", result[1].Word);
        }

        [Test]
        public void LookupWeighted_Mic_StartMatchAlpha_ReturnsMicrosoftMicrospot()
        {
            var result = wordsLookupService.LookupStartMatchAlphabetical("mic", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Microsoft", result[0].Word);
            Assert.AreEqual("Microspot", result[1].Word);
        }

        [Test]
        public void LookupWeighted_Micr_StartMatchAlpha_ReturnsMicrophoneMicrospot()
        {
            var result = wordsLookupService.LookupStartMatchAlphabetical("micr", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Microphone", result[0].Word);
            Assert.AreEqual("Microspot", result[1].Word);
        }

        [Test]
        public void LookupWeighted_Mic_ContainingMatchAlpha_ReturnsAntimicrobialAtomicMimicry()
        {
            var result = wordsLookupService.LookupContainingMatchAlphabetical("mic", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Antimicrobial", result[0].Word);
            Assert.AreEqual("Atomic", result[1].Word);
            Assert.AreEqual("Mimicry", result[2].Word);
        }

        [Test]
        public void LookupWeighted_Micr_ContainingMatchAlpha_ReturnsAntimicrobialMimicry()
        {
            var result = wordsLookupService.LookupContainingMatchAlphabetical("micr", 5).Result;

            Assert.NotNull(result);
            Assert.AreEqual("Antimicrobial", result[0].Word);
            Assert.AreEqual("Mimicry", result[1].Word);
        }

        private void LoadTestData(TestAppDbContext dbContext)
        {
            var word1 = new LookupWord { Word = "Microphone" };
            var word2 = new LookupWord { Word = "Microsoft" };
            var word3 = new LookupWord { Word = "Microscope" };
            var word4 = new LookupWord { Word = "Microspot" };
            var word5 = new LookupWord { Word = "Antimicrobial" };
            var word6 = new LookupWord { Word = "Atomic" };
            var word7 = new LookupWord { Word = "Mimicry" };

            dbContext.LookupWords.Add(word1);
            dbContext.LookupWords.Add(word2);
            dbContext.LookupWords.Add(word3);
            dbContext.LookupWords.Add(word4);
            dbContext.LookupWords.Add(word5);
            dbContext.LookupWords.Add(word6);
            dbContext.LookupWords.Add(word7);

            dbContext.SaveChanges();

            var searchString1 = new SearchString { LookupWordId = word1.Id, String = "mic", Weight = 10 };
            var searchString2 = new SearchString { LookupWordId = word3.Id, String = "mic", Weight = 5 };
            var searchString3 = new SearchString { LookupWordId = word3.Id, String = "micr", Weight = 11 };
            var searchString4 = new SearchString { LookupWordId = word2.Id, String = "micr", Weight = 1 };

            dbContext.SearchStrings.Add(searchString1);
            dbContext.SearchStrings.Add(searchString2);
            dbContext.SearchStrings.Add(searchString3);
            dbContext.SearchStrings.Add(searchString4);

            dbContext.SaveChanges();
        }
    }
}
