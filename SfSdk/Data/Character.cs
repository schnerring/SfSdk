using System;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Response;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="ICharacter"/>.
    /// </summary>
    internal class Character : ICharacter
    {
        private readonly int _constitution;
        private readonly double _criticalHit;
        private readonly int _damageMax;
        private readonly int _damageMin;
        private readonly int _defense;
        private readonly int _dexterity;
        private readonly int _evasion;
        private readonly int _hitPoints;
        private readonly int _intelligence;
        private readonly int _luck;
        private readonly int _rank;
        private readonly int _resistance;
        private readonly int _strength;
        private readonly string _username;
        
        /// <summary>
        ///     Creates a new <see cref="Character" /> instance, calculated from a <see cref="CharacterResponse" />.
        /// </summary>
        /// <param name="response">The <see cref="CharacterResponse" /> from which arguments the <see cref="Character" /> is going to calculated.</param>
        public Character(ICharacterResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            if (response.Savegame == null) throw new ArgumentException("Character response must contain a savegame.", "response");

            ISavegame sg = response.Savegame;
            _strength = sg.GetValue(SF.SgAttrStaerke) + sg.GetValue(SF.SgAttrStaerkeBonus);
            _dexterity = sg.GetValue(SF.SgAttrBeweglichkeit) + sg.GetValue(SF.SgAttrBeweglichkeitBonus);
            _constitution = sg.GetValue(SF.SgAttrIntelligenz) + sg.GetValue(SF.SgAttrIntelligenzBonus);     // mistake in
            _intelligence = sg.GetValue(SF.SgAttrAusdauer) + sg.GetValue(SF.SgAttrAusdauerBonus);           // original source code
            _luck = sg.GetValue(SF.SgAttrWillenskraft) + sg.GetValue(SF.SgAttrWillenskraftBonus);

            int level = sg.GetValue(SF.SgLevel);
            int potionGain = sg.GetValue(SF.SgPotionGain);
            var charClass = (SfClass) sg.GetValue(SF.SgClass);
            int damageMin = sg.GetValue(SF.SgDamageMin);
            int damageMax = sg.GetValue(SF.SgDamageMax);

            _defense = _strength/2;
            _evasion = _dexterity/2;
            _resistance = _intelligence/2;

            _criticalHit = Math.Round(_luck*5/(double) (level*2), 2);
            if (_criticalHit < 0) _criticalHit = 0;
            else if (_criticalHit > 50) _criticalHit = 50;

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
                    tmpDamageFactor = 1 + (double) _strength/10;
                    tmpLifeFactor = 5;
                    break;
                case SfClass.Mage:
                    tmpDamageFactor = 1 + (double) _intelligence/10;
                    tmpLifeFactor = 2;
                    break;
                case SfClass.Scout:
                    tmpDamageFactor = 1 + (double) _dexterity/10;
                    tmpLifeFactor = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _damageMin = (int) Math.Round(damageMin*tmpDamageFactor);
            _damageMax = (int) Math.Round(damageMax*tmpDamageFactor);
            _hitPoints =
                (int)
                Math.Round((double) tmpLifeFactor*_constitution*(1 + level)*
                           (tmpHealth > 0 ? 1 + tmpHealth*0.01 : 1));
        }

        public string Username
        {
            get { return _username; }
        }

        public int Rank
        {
            get { return _rank; }
        }

        public int Strength
        {
            get { return _strength; }
        }

        public int Dexterity
        {
            get { return _dexterity; }
        }

        public int Intelligence
        {
            get { return _intelligence; }
        }

        public int Constitution
        {
            get { return _constitution; }
        }

        public int Luck
        {
            get { return _luck; }
        }

        public int Defense
        {
            get { return _defense; }
        }

        public int Evasion
        {
            get { return _evasion; }
        }

        public int Resistance
        {
            get { return _resistance; }
        }

        public int DamageMin
        {
            get { return _damageMin; }
        }

        public int DamageMax
        {
            get { return _damageMax; }
        }

        public int HitPoints
        {
            get { return _hitPoints; }
        }

        public double CriticalHit
        {
            get { return _criticalHit; }
        }

        public Task Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
