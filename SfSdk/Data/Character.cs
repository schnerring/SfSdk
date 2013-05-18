using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Response;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="ICharacter" />.
    /// </summary>
    [DebuggerDisplay("{Rank}, {Level}, {Honor}, {Username}, {Guild}")]
    internal class Character : ICharacter
    {
        private readonly string _guild;
        private readonly string _username;
        private bool _loaded;

        /// <summary>
        ///     Creates a new <see cref="Character" /> instance, calculated from a <see cref="CharacterResponse" />.
        /// </summary>
        /// <param name="response">
        ///     The <see cref="CharacterResponse" /> from which arguments the <see cref="Character" /> is going to calculated.
        /// </param>
        /// <param name="username">The username of the character.</param>
        public Character(ICharacterResponse response, string username)
        {
            if (response == null) throw new ArgumentNullException("response");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username must not be null or empty.", "username");
            if (response.Savegame == null)
                throw new ArgumentException("Character response must contain a savegame.", "response");

            _username = username;
            _guild = response.Guild;
            LoadFromSavegame(response.Savegame);
        }

        /// <summary>
        ///     Creates a new <see cref="Character" /> instance, without its details loaded. />.
        /// </summary>
        /// <param name="rank">The character's rank.</param>
        /// <param name="username">The character's username.</param>
        /// <param name="guild">The character's guild.</param>
        /// <param name="level">The character's level.</param>
        /// <param name="honor">The character's honor.</param>
        public Character(int rank, string username, string guild, int level, int honor)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username must not be null or empty.", "username");
            Rank = rank;
            _username = username;
            _guild = guild;
            Level = level;
            Honor = honor;
        }

        public Task Refresh()
        {
            throw new NotImplementedException();
        }

        private void LoadFromSavegame(ISavegame sg)
        {
            Rank = sg.GetValue(SF.SgRank);
            Level = sg.GetValue(SF.SgLevel);
            Honor = sg.GetValue(SF.SgHonor);

            Strength = sg.GetValue(SF.SgAttrStaerke) + sg.GetValue(SF.SgAttrStaerkeBonus);
            Dexterity = sg.GetValue(SF.SgAttrBeweglichkeit) + sg.GetValue(SF.SgAttrBeweglichkeitBonus);
            Constitution = sg.GetValue(SF.SgAttrIntelligenz) + sg.GetValue(SF.SgAttrIntelligenzBonus);  // mistake in
            Intelligence = sg.GetValue(SF.SgAttrAusdauer) + sg.GetValue(SF.SgAttrAusdauerBonus);        // original source code
            Luck = sg.GetValue(SF.SgAttrWillenskraft) + sg.GetValue(SF.SgAttrWillenskraftBonus);

            int level = sg.GetValue(SF.SgLevel);
            int potionGain = sg.GetValue(SF.SgPotionGain);
            var charClass = (SfClass) sg.GetValue(SF.SgClass);
            int damageMin = sg.GetValue(SF.SgDamageMin);
            int damageMax = sg.GetValue(SF.SgDamageMax);

            Defense = Strength/2;
            Evasion = Dexterity/2;
            Resistance = Intelligence/2;

            CriticalHit = Math.Round(Luck*5/(double) (level*2), 2);
            if (CriticalHit < 0) CriticalHit = 0;
            else if (CriticalHit > 50) CriticalHit = 50;

            int tmpHealth = 0;
            for (int i = 0; i < 3; i++)
            {
                int potionType = sg.GetValue(SF.SgPotionType + i);
                if (potionType == 16)
                    tmpHealth = potionGain;
            }

            double tmpDamageFactor;
            int tmpLifeFactor;

            switch (charClass)
            {
                case SfClass.Warrior:
                    tmpDamageFactor = 1 + (double) Strength/10;
                    tmpLifeFactor = 5;
                    break;
                case SfClass.Mage:
                    tmpDamageFactor = 1 + (double) Intelligence/10;
                    tmpLifeFactor = 2;
                    break;
                case SfClass.Scout:
                    tmpDamageFactor = 1 + (double) Dexterity/10;
                    tmpLifeFactor = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DamageMin = (int) Math.Round(damageMin*tmpDamageFactor);
            DamageMax = (int) Math.Round(damageMax*tmpDamageFactor);
            HitPoints =
                (int)
                Math.Round((double) tmpLifeFactor*Constitution*(1 + level)*
                           (tmpHealth > 0 ? 1 + tmpHealth*0.01 : 1));
            _loaded = true;
        }

        public string Guild
        {
            get { return _guild; }
        }

        public string Username
        {
            get { return _username; }
        }

        public int Rank { get; private set; }

        public int Level { get; private set; }

        public int Honor { get; private set; }

        public int Strength { get; private set; }

        public int Dexterity { get; private set; }

        public int Intelligence { get; private set; }

        public int Constitution { get; private set; }

        public int Luck { get; private set; }

        public int Defense { get; private set; }

        public int Evasion { get; private set; }

        public int Resistance { get; private set; }

        public int DamageMin { get; private set; }

        public int DamageMax { get; private set; }

        public int HitPoints { get; private set; }

        public double CriticalHit { get; private set; }
    }
}