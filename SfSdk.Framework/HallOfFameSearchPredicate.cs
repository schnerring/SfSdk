using System.Collections.Generic;

namespace SfSdk.Framework
{
    public class HallOfFameSearchPredicate
    {
        public HallOfFameSearchPredicate()
        {
            MinRank = 1;
            MinHonor = 1;
            MinLevel = 1;
            MaxRank = 1;
            MaxHonor = 1;
            MaxLevel = 1;
            ExcludedGuilds = new List<string>();
            ExcludedUsernames = new List<string>();
        }

        public int MinRank { get; set; }
        public int MaxRank { get; set; }
        public int MinHonor { get; set; }
        public int MaxHonor { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public List<string> ExcludedGuilds { get; set; }
        public List<string> ExcludedUsernames { get; set; }
    }
}