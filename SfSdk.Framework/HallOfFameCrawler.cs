using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Framework
{
    /// <summary>
    ///     A class which provides extended features for crawling the hall of fame.
    /// </summary>
    public class HallOfFameCrawler
    {
        private readonly ISession _session;

        /// <summary>
        ///     Creates a new instance of <see cref="HallOfFameCrawler" />.
        /// </summary>
        /// <param name="session">The session which provides access to the S&amp;F data.</param>
        public HallOfFameCrawler(ISession session)
        {
            _session = session;
        }

        /// <summary>
        ///     Searches the hall of fame given on a specific search predicate.
        /// </summary>
        /// <param name="searchPredicate">The search predicate used for the search.</param>
        public async Task<IEnumerable<ICharacter>> Search(HallOfFameSearchPredicate searchPredicate)
        {
            if (searchPredicate == null) throw new ArgumentNullException("searchPredicate");
            if (searchPredicate.MinRank < 1)
                throw new ArgumentException("MinRank must be greater than zero.", "searchPredicate");
            if (searchPredicate.MinHonor < 1)
                throw new ArgumentException("MinHonor must be greater than zero.", "searchPredicate");
            if (searchPredicate.MinLevel < 1)
                throw new ArgumentException("MinLevel must be greater than zero.", "searchPredicate");
            if (searchPredicate.MaxRank < searchPredicate.MinRank)
                throw new ArgumentException("MaxRank must be greater than MinRank.", "searchPredicate");
            if (searchPredicate.MaxHonor < searchPredicate.MinHonor)
                throw new ArgumentException("MaxHonor must be greater than MinHonor.", "searchPredicate");
            if (searchPredicate.MaxLevel < searchPredicate.MinLevel)
                throw new ArgumentException("MaxLevel must be greater than MinLevel.", "searchPredicate");
            
            var characters =  await _session.HallOfFameAsync();

            return
                characters.Where(
                    c =>
                    c.Rank >= searchPredicate.MinRank && c.Rank <= searchPredicate.MaxRank &&
                    c.Honor >= searchPredicate.MinHonor && c.Honor <= searchPredicate.MaxHonor &&
                    c.Level >= searchPredicate.MinLevel && c.Level <= searchPredicate.MaxLevel);
        }
    }
}