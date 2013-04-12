using System;
using System.Threading.Tasks;
using SfSdk.Contracts;
using SfSdk.Enums;

namespace SfSdk.ResponseData
{
    internal class CharacterResponse : IResponse, ICharacter
    {
        private readonly string _comment;
        private readonly string _guild;

        internal CharacterResponse(string[] savegameParts, string guild, string comment)
        {
            var sg = new Savegame(savegameParts);

            // not from savegame
            _guild = guild;
            _comment = comment;

            Strength = sg.Strength + sg.StrengthBonus;
            Dexterity = sg.Dexterity + sg.DexterityBonus;
            Intelligence = sg.Intelligence + sg.IntelligenceBonus;
            Constitution = sg.Constitution + sg.ConstitutionBonus;
            Luck = sg.Luck + sg.LuckBonus;

            Defense = Strength/2;
            Evasion = Dexterity/2;
            Resistance = Intelligence/2;

            CriticalHit = Math.Round(Luck*5/(double) (sg.Level*2), 2);
            if (CriticalHit < 0) CriticalHit = 0;
            else if (CriticalHit > 50) CriticalHit = 50;

            int tmpHealth = 0;
            for (int i = 0; i < 3; i++)
            {
                int potionType = int.Parse(savegameParts[(int) SfSavegame.PotionType + i]);
                if (potionType == 16)
                    tmpHealth = sg.PotionGain;
            }

            double tmpDamageFactor;
            int tmpLifeFactor;

            switch (sg.Class)
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

            DamageMin = (int) Math.Round(sg.DamageMin*tmpDamageFactor);
            DamageMax = (int) Math.Round(sg.DamageMax*tmpDamageFactor);
            HitPoints =
                (int)
                Math.Round((double) tmpLifeFactor*Constitution*(1 + sg.Level)*
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