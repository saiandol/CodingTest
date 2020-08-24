using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly int _shirtsCount;
        private readonly ILookup<Guid, Shirt> _lookupGroupedByColor;
        private readonly ILookup<Guid, Shirt> _lookupGroupedBySize;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirtsCount = shirts.Count;

            _lookupGroupedByColor = shirts.ToLookup(s => s.Color.Id);
            _lookupGroupedBySize = shirts.ToLookup(s => s.Size.Id);

        }


        public SearchResults Search(SearchOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "SearchOptions cannot be null");
            }
            return new SearchResults()
            {
                Shirts = GetResultantShirtsMatchingOptions(options),
                ColorCounts = GetShirtCountByColor(options),
                SizeCounts = GetShirtCountBySize(options)
            };
            
        }

        private List<Shirt> GetResultantShirtsMatchingOptions(SearchOptions options)
        {
            var resultantShirts = new List<Shirt>(_shirtsCount);
            if (ThereOnlyColorOptions(options))
            {
                foreach (var optionsColor in options.Colors)
                {
                    resultantShirts.AddRange(_lookupGroupedByColor[optionsColor.Id]);
                }

                return resultantShirts;
            }

            if (ThereAreOnlySizeOptions(options))
            {
                foreach (var size in options.Sizes)
                {
                    resultantShirts.AddRange(_lookupGroupedBySize[size.Id]);
                }

                return resultantShirts;
            }


            if (ThereAreBothSizeAndColorOptions(options))
            {
                foreach (var optionsColor in options.Colors)
                {
                    resultantShirts.AddRange(_lookupGroupedByColor[optionsColor.Id].Where(shirt => options.Sizes.Select(s => s.Id).Contains(shirt.Size.Id)));
                }

                return resultantShirts;
            }

            ;
            return resultantShirts;
        }

        private static bool ThereAreBothSizeAndColorOptions(SearchOptions options)
        {
            return options.Colors != null && options.Sizes != null;
        }

        private static bool ThereAreOnlySizeOptions(SearchOptions options)
        {
            return options.Sizes != null && options.Sizes.Any() && (options.Colors == null || !options.Colors.Any());
        }

        private static bool ThereOnlyColorOptions(SearchOptions options)
        {
            return options.Colors != null && options.Colors.Any() && (options.Sizes == null || !options.Sizes.Any());
        }


        private  List<ColorCount> GetShirtCountByColor(SearchOptions options)
        {
            return Color.All.Select(color => new ColorCount { Color = color, Count = _lookupGroupedByColor[color.Id].Count(shirt => shirt.Color.Id == color.Id && (!options.Sizes.Any() || options.Sizes.Select(s => s.Id).Contains(shirt.Size.Id)))}).ToList();
        }

        private List<SizeCount> GetShirtCountBySize(SearchOptions options)
        {
            return Size.All.Select(size => new SizeCount { Size = size, Count = _lookupGroupedBySize[size.Id].Count(shirt => shirt.Size.Id == size.Id && (!options.Colors.Any() || options.Colors.Select(c => c.Id).Contains(shirt.Color.Id))) }).ToList();
        }
    }
}