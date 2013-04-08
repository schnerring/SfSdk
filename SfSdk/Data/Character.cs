using System;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    internal class Character : ICharacter
    {
        private readonly string _comment;
        private readonly string _guild;
        private readonly Class _sgClass;
        private readonly int _sgConstitution;
        private readonly int _sgConstitutionBonus;
        private readonly int _sgConstitutionBought;
        private readonly int _sgDamageMax;
        private readonly int _sgDamageMin;
        private readonly int _sgDexterity;
        private readonly int _sgDexterityBonus;
        private readonly int _sgDexterityBought;
        private readonly int _sgIntelligence;
        private readonly int _sgIntelligenceBonus;
        private readonly int _sgIntelligenceBought;
        private readonly int _sgLevel;
        private readonly int _sgLuck;
        private readonly int _sgLuckBonus;
        private readonly int _sgLuckBought;
        private readonly int _sgPotionGain;
        private readonly int _sgPotionType;
        private readonly int _sgStrength;
        private readonly int _sgStrengthBonus;
        private readonly int _sgStrengthBought;

        internal Character(string[] savegameParts, string guild, string comment)
        {
            _sgLevel = int.Parse(savegameParts[(int) SfSavegame.Level]);
//            _gold = int.Parse(savegameParts[(int) SfSavegame.Gold]);
//            _mushrooms = int.Parse(savegameParts[(int) SfSavegame.Mushrooms]);
            _sgClass = (Class) int.Parse(savegameParts[(int) SfSavegame.Class]);
            _sgStrength = int.Parse(savegameParts[(int) SfSavegame.Strength]);
            _sgDexterity = int.Parse(savegameParts[(int) SfSavegame.Dexterity]);
            _sgConstitution = int.Parse(savegameParts[(int) SfSavegame.Constitution]);
            _sgIntelligence = int.Parse(savegameParts[(int) SfSavegame.Intelligence]);
            _sgLuck = int.Parse(savegameParts[(int) SfSavegame.Luck]);
            _sgStrengthBonus = int.Parse(savegameParts[(int) SfSavegame.StrengthBonus]);
            _sgDexterityBonus = int.Parse(savegameParts[(int) SfSavegame.DexterityBonus]);
            _sgConstitutionBonus = int.Parse(savegameParts[(int) SfSavegame.ConstitutionBonus]);
            _sgIntelligenceBonus = int.Parse(savegameParts[(int) SfSavegame.IntelligenceBonus]);
            _sgLuckBonus = int.Parse(savegameParts[(int) SfSavegame.LuckBonus]);
            _sgStrengthBought = int.Parse(savegameParts[(int) SfSavegame.StrengthBought]);
            _sgDexterityBought = int.Parse(savegameParts[(int) SfSavegame.DexterityBought]);
            _sgConstitutionBought = int.Parse(savegameParts[(int) SfSavegame.ConstitutionBought]);
            _sgIntelligenceBought = int.Parse(savegameParts[(int) SfSavegame.IntelligenceBought]);
            _sgLuckBought = int.Parse(savegameParts[(int) SfSavegame.LuckBought]);
            _sgDamageMin = int.Parse(savegameParts[(int) SfSavegame.DamageMin]);
            _sgDamageMax = int.Parse(savegameParts[(int) SfSavegame.DamageMax]);
            _sgPotionGain = int.Parse(savegameParts[(int) SfSavegame.PotionGain]);

            // not from savegame
            _guild = guild;
            _comment = comment;

            Strength = _sgStrength + _sgStrengthBonus;
            Dexterity = _sgDexterity + _sgDexterityBonus;
            Intelligence = _sgIntelligence + _sgIntelligenceBonus;
            Constitution = _sgConstitution + _sgConstitutionBonus;
            Luck = _sgLuck + _sgLuckBonus;

            Defense = Strength/2;
            Evasion = Dexterity/2;
            Resistance = Intelligence/2;

            CriticalHit = Math.Round(Luck*5/(double) (_sgLevel*2), 2);
            if (CriticalHit < 0) CriticalHit = 0;
            else if (CriticalHit > 50) CriticalHit = 50;

            int tmpHealth = 0;
            for (int i = 0; i < 3; i++)
            {
                int potionType = int.Parse(savegameParts[(int) SfSavegame.PotionType + i]);
                if (potionType == 16)
                    tmpHealth = _sgPotionGain;
            }

            double tmpDamageFactor;
            int tmpLifeFactor;

            switch (_sgClass)
            {
                case Class.Warrior:
                    tmpDamageFactor = 1 + (double) Strength/10;
                    tmpLifeFactor = 5;
                    break;
                case Class.Mage:
                    tmpDamageFactor = 1 + (double) Intelligence/10;
                    tmpLifeFactor = 2;
                    break;
                case Class.Scout:
                    tmpDamageFactor = 1 + (double) Dexterity/10;
                    tmpLifeFactor = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DamageMin = (int) Math.Round(_sgDamageMin*tmpDamageFactor);
            DamageMax = (int) Math.Round(_sgDamageMax*tmpDamageFactor);
            HitPoints =
                (int)
                Math.Round((double) tmpLifeFactor*Constitution*(1 + _sgLevel)*
                           (tmpHealth > 0 ? 1 + tmpHealth*0.01 : 1));
        }

        public string Username { get; private set; } // TODO
        public int Rank { get; private set; } // TODO
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
        public Task Reresh()
        {
            throw new NotImplementedException();
        }
    }
}