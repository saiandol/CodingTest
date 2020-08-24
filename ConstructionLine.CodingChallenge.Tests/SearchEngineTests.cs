using System;
using System.Collections.Generic;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        private List<Shirt> _sampleShirtsData;

        [SetUp]
        public void Setup()
        {
            _sampleShirtsData = new SampleDataBuilder(20).CreateShirts();
        }

        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }
        
        [Test]
        public void Test_NoSizeOptions()
        {

            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }
        
        [Test]
        public void Test_NoColorOptions()
        {
            

            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }
        
        [Test]
        public void Test_NoColorOrSizeOptions()
        {
            

            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }
        
        [Test]
        public void Test_AllColorOptionsOnly()
        {
            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
                Colors = Color.All
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }
        
        [Test]
        public void Test_AllSizeOptionsOnly()
        {
            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
                Sizes = Size.All
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }

        [Test]
        public void Test_AllSizeOptions_And_AllColorOptions()
        {
            var searchEngine = new SearchEngine(_sampleShirtsData);

            var searchOptions = new SearchOptions
            {
                Sizes = Size.All,
                Colors = Color.All
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_sampleShirtsData, searchOptions, results.SizeCounts);
            AssertColorCounts(_sampleShirtsData, searchOptions, results.ColorCounts);
        }

        [Test]
        public void Given_SearchOptions_IsNull_Throws_ArguementNullException()
        {
            var searchEngine = new SearchEngine(_sampleShirtsData);

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(null));
        }
    }
}
