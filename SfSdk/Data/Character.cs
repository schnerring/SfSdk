using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Providers;
using SfSdk.Response;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="ICharacter" />.
    /// </summary>
    [DebuggerDisplay("{Rank}, {Level}, {Honor}, {Username}, {Guild}")]
    internal class Character : ICharacter, INotifyPropertyChanged
    {
        private readonly string _guild;
        private readonly ISession _session;
        private readonly string _username;
        private int _constitution;
        private double _criticalHit;
        private int _damageMax;
        private int _damageMin;
        private int _defense;
        private int _dexterity;
        private int _evasion;
        private int _hitPoints;
        private int _honor;
        private int _intelligence;
        private bool _isLoaded;
        private int _level;
        private int _luck;
        private int _rank;
        private int _resistance;
        private int _strength;
//        private ScrapbookItemProvider _itemProvider;
//        private List<IScrapbookItem> _inventoryItems;

        /// <summary>
        ///     Creates a new <see cref="Character" /> instance, calculated from a <see cref="CharacterResponse" />. The character's loaded status is initially set to true.
        /// </summary>
        /// <param name="response">The <see cref="CharacterResponse" /> from which arguments the <see cref="Character" /> is going to calculated.</param>
        /// <param name="username">The username of the character.</param>
        /// <param name="session">The session, where the character is going to be attatched to.</param>
        /// <exception cref="ArgumentNullException">When savegame is empty or username is null or empty.</exception>
        /// <exception cref="ArgumentException">When session or response is null.</exception>
        public Character(ICharacterResponse response, string username, ISession session)
        {
            if (response == null)
                throw new ArgumentNullException("response");
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username must not be null or empty.", "username");
            if (response.Savegame == null)
                throw new ArgumentException("Character response must contain a savegame.", "response");
            if (session == null)
                throw new ArgumentNullException("session");

            _username = username;
            _session = session;
            _guild = response.Guild;
//            _itemProvider = new ScrapbookItemProvider(_session.ServerUri);
            LoadFromSavegame(response.Savegame);
            IsLoaded = true;
        }


        /// <summary>
        ///     Creates a new <see cref="Character" /> instance, without its details loaded.
        /// </summary>
        /// <param name="rank">The character's rank.</param>
        /// <param name="username">The character's username.</param>
        /// <param name="guild">The character's guild.</param>
        /// <param name="level">The character's level.</param>
        /// <param name="honor">The character's honor.</param>
        /// <param name="session">The session, where the character is going to be attatched to.</param>
        /// <exception cref="ArgumentException">When username is null or empty.</exception>
        /// <exception cref="ArgumentNullException">When session is null.</exception>
        public Character(int rank, string username, string guild, int level, int honor, ISession session)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username must not be null or empty.", "username");
            if (session == null) throw new ArgumentNullException("session");
            Rank = rank;
            _username = username;
            _guild = guild;
            _session = session;
            Level = level;
            Honor = honor;
        }

        /// <summary>
        ///     Refreshes the data of the character by requesting it again.
        /// </summary>
        /// <param name="force">Indicates whether request shall be forced, even if the character has already been loaded.</param>
        /// <returns>Task.</returns>
        public async Task Refresh(bool force = false)
        {
            if (_isLoaded && !force) return;
            ICharacter character = await _session.RequestCharacterAsync(_username);
            this.CopyPropertiesFrom(character);
            IsLoaded = true;
        }

        public string Guild
        {
            get { return _guild; }
        }

        public string Username
        {
            get { return _username; }
        }

        public int Rank
        {
            get { return _rank; }
            private set
            {
                if (value == _rank) return;
                _rank = value;
                NotifyOfPropertyChange();
            }
        }

        public int Level
        {
            get { return _level; }
            private set
            {
                if (value == _level) return;
                _level = value;
                NotifyOfPropertyChange();
            }
        }

        public int Honor
        {
            get { return _honor; }
            private set
            {
                if (value == _honor) return;
                _honor = value;
                NotifyOfPropertyChange();
            }
        }

        public int Strength
        {
            get { return _strength; }
            private set
            {
                if (value == _strength) return;
                _strength = value;
                NotifyOfPropertyChange();
            }
        }

        public int Dexterity
        {
            get { return _dexterity; }
            private set
            {
                if (value == _dexterity) return;
                _dexterity = value;
                NotifyOfPropertyChange();
            }
        }

        public int Intelligence
        {
            get { return _intelligence; }
            private set
            {
                if (value == _intelligence) return;
                _intelligence = value;
                NotifyOfPropertyChange();
            }
        }

        public int Constitution
        {
            get { return _constitution; }
            private set
            {
                if (value == _constitution) return;
                _constitution = value;
                NotifyOfPropertyChange();
            }
        }

        public int Luck
        {
            get { return _luck; }
            private set
            {
                if (value == _luck) return;
                _luck = value;
                NotifyOfPropertyChange();
            }
        }

        public int Defense
        {
            get { return _defense; }
            private set
            {
                if (value == _defense) return;
                _defense = value;
                NotifyOfPropertyChange();
            }
        }

        public int Evasion
        {
            get { return _evasion; }
            private set
            {
                if (value == _evasion) return;
                _evasion = value;
                NotifyOfPropertyChange();
            }
        }

        public int Resistance
        {
            get { return _resistance; }
            private set
            {
                if (value == _resistance) return;
                _resistance = value;
                NotifyOfPropertyChange();
            }
        }

        public int DamageMin
        {
            get { return _damageMin; }
            private set
            {
                if (value == _damageMin) return;
                _damageMin = value;
                NotifyOfPropertyChange();
            }
        }

        public int DamageMax
        {
            get { return _damageMax; }
            private set
            {
                if (value == _damageMax) return;
                _damageMax = value;
                NotifyOfPropertyChange();
            }
        }

        public int HitPoints
        {
            get { return _hitPoints; }
            private set
            {
                if (value == _hitPoints) return;
                _hitPoints = value;
                NotifyOfPropertyChange();
            }
        }

        public double CriticalHit
        {
            get { return _criticalHit; }
            private set
            {
                if (value.Equals(_criticalHit)) return;
                _criticalHit = value;
                NotifyOfPropertyChange();
            }
        }

//        public List<IScrapbookItem> InventoryItems
//        {
//            get { return _inventoryItems; }
//            set
//            {
//                _inventoryItems = value;
//                NotifyOfPropertyChange();
//            }
//        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (value == _isLoaded) return;
                _isLoaded = value;
                NotifyOfPropertyChange();
            }
        }

        private void LoadFromSavegame(ISavegame sg)
        {
            Rank = sg.GetValue(SF.SgRank);
            Level = sg.GetValue(SF.SgLevel);
            Honor = sg.GetValue(SF.SgHonor);

            Strength = sg.GetValue(SF.SgAttrStaerke) + sg.GetValue(SF.SgAttrStaerkeBonus);
            Dexterity = sg.GetValue(SF.SgAttrBeweglichkeit) + sg.GetValue(SF.SgAttrBeweglichkeitBonus);
            Constitution = sg.GetValue(SF.SgAttrIntelligenz) + sg.GetValue(SF.SgAttrIntelligenzBonus); // mistake in
            Intelligence = sg.GetValue(SF.SgAttrAusdauer) + sg.GetValue(SF.SgAttrAusdauerBonus); // original source code
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

//            InventoryItems = _itemProvider.CreateInventoryItems(sg);
        }

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}