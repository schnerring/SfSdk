using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Framework
{
    public delegate void ChunkCompletedEventHandler(HallOfFameCrawler sender, ChunkCompletedEventArgs args);

    /// <summary>
    ///     A class which provides extended features for crawling the hall of fame.
    /// </summary>
    public class HallOfFameCrawler
    {
        private readonly ISession _session;

        public event ChunkCompletedEventHandler ChunkCompleted;

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
        public async Task<IEnumerable<ICharacter>> SearchAsync(HallOfFameSearchPredicate searchPredicate)
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

            const int chunkOffset = 7;
            const int chunkSize = 2*chunkOffset + 1;

            // the algorithm processes the list from back to front.
            var maxRankCap = searchPredicate.MaxRank - chunkOffset;
            var minRankCap = searchPredicate.MinRank + chunkOffset;

            // search range <= chunkSize
            if (maxRankCap < 1) maxRankCap = 1;
            if (maxRankCap < minRankCap) maxRankCap = minRankCap;

            var results = new List<ICharacter>();

            // get front side chunk
            var firstChunk = await RequestHallOfFame(minRankCap);
            // remove possible overhead
            firstChunk.RemoveAll(c => c.Rank > searchPredicate.MaxRank);
            results.AddRange(firstChunk);

            // search range <= chunkSize
            if (maxRankCap == minRankCap) return results;

            // get back side chunk
            var lastChunk = await RequestHallOfFame(maxRankCap);
            var maxFirstChunkRank = results.Last().Rank;
            if (lastChunk.Any(c => c.Rank <= maxFirstChunkRank))
            {
                // front and back side chunk overlap already
                lastChunk.RemoveAll(c => c.Rank <= maxFirstChunkRank);
                results.AddRange(lastChunk);
                return results;
            }
            results.AddRange(lastChunk);

            var totalCharacterCount = searchPredicate.MaxRank - searchPredicate.MinRank + 1;
            var remainingCharacters = totalCharacterCount - 2*chunkSize;
            var fullChunks = remainingCharacters/chunkSize;
            var partialChunkSize = remainingCharacters%chunkSize;
            var chunksToBeProcessed = fullChunks + (partialChunkSize > 0 ? 1 : 0);
            var tasksCompleted = 0;

            Func<int, Task<List<ICharacter>>> getChunk = async currentChunkIndex =>
            {
                var charactersChunk = await RequestHallOfFame(maxRankCap - currentChunkIndex * chunkSize);

                // no overlap
                if (currentChunkIndex != chunksToBeProcessed) return charactersChunk;

                // remove possible overlap of last chunk
                if (partialChunkSize > 0)
                    charactersChunk.RemoveRange(0, chunkSize - partialChunkSize);

                return charactersChunk;
            };

            var tasks = new Task<List<ICharacter>>[chunksToBeProcessed];
            for (var i = 1; i <= chunksToBeProcessed; ++i)
            {
                var task = getChunk(i).ContinueWith(t =>
                {
                    var args = new ChunkCompletedEventArgs(++tasksCompleted + 2, chunksToBeProcessed + 2);
                    ChunkCompleted(this, args);
                    return t.Result;
                });
                tasks[i - 1] = task;
            }

            var characterChunks = await Task.WhenAll(tasks);

            var characters = characterChunks.SelectMany(c => c);
            results.AddRange(characters);

            return results.OrderBy(c => c.Rank);
        }

        private async Task<List<ICharacter>> RequestHallOfFame(int searchRank)
        {
            return (await _session.HallOfFameAsync(searchRank.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }

    public class ChunkCompletedEventArgs
    {
        public int FinishedChunks { get; private set; }
        public int TotalChunks { get; private set; }

        public ChunkCompletedEventArgs(int finishedChunks, int totalChunks)
        {
            FinishedChunks = finishedChunks;
            TotalChunks = totalChunks;
        }
    }
}