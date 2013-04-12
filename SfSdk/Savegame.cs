using SfSdk.Enums;

namespace SfSdk
{
    internal class Savegame
    {
        private readonly SfClass _class;
        private readonly int _constitution;
        private readonly int _constitutionBonus;
        private readonly int _constitutionBought;
        private readonly int _damageMax;
        private readonly int _damageMin;
        private readonly int _dexterity;
        private readonly int _dexterityBonus;
        private readonly int _dexterityBought;
        private readonly int _intelligence;
        private readonly int _intelligenceBonus;
        private readonly int _intelligenceBought;
        private readonly int _level;
        private readonly int _luck;
        private readonly int _luckBonus;
        private readonly int _luckBought;
        private readonly int _potionGain;
        private readonly int _strength;
        private readonly int _strengthBonus;
        private readonly int _strengthBought;

        public Savegame(string[] savegameParts)
        {
            _level = int.Parse(savegameParts[(int) SfSavegame.Level]);
//            _gold = int.Parse(savegameParts[(int) SfSavegame.Gold]);
//            _mushrooms = int.Parse(savegameParts[(int) SfSavegame.Mushrooms]);
            _class = (SfClass) int.Parse(savegameParts[(int) SfSavegame.Class]);
            _strength = int.Parse(savegameParts[(int) SfSavegame.AttrStaerke]);
            _dexterity = int.Parse(savegameParts[(int) SfSavegame.AttrBeweglichkeit]);                  // other way round in
            _constitution = int.Parse(savegameParts[(int) SfSavegame.AttrIntelligenz]);                 // original source code
            _intelligence = int.Parse(savegameParts[(int) SfSavegame.AttrAusdauer]);
            _luck = int.Parse(savegameParts[(int) SfSavegame.AttrWillenskraft]);
            _strengthBonus = int.Parse(savegameParts[(int) SfSavegame.AttrStaerkeBonus]);
            _dexterityBonus = int.Parse(savegameParts[(int) SfSavegame.AttrBeweglichkeitBonus]);
            _constitutionBonus = int.Parse(savegameParts[(int) SfSavegame.AttrIntelligenzBonus]);       // other way round in
            _intelligenceBonus = int.Parse(savegameParts[(int) SfSavegame.AttrAusdauerBonus]);          // original source code
            _luckBonus = int.Parse(savegameParts[(int) SfSavegame.AttrWillenskraftBonus]);
            _strengthBought = int.Parse(savegameParts[(int) SfSavegame.AttrStaerkeGekauft]);
            _dexterityBought = int.Parse(savegameParts[(int) SfSavegame.AttrBeweglichkeitGekauft]);
            _constitutionBought = int.Parse(savegameParts[(int) SfSavegame.AttrIntelligenzGekauft]);    // other way round in
            _intelligenceBought = int.Parse(savegameParts[(int) SfSavegame.AttrAusdauerGekauft]);       // original source code
            _luckBought = int.Parse(savegameParts[(int) SfSavegame.AttrWillenskraftGekauft]);
            _damageMin = int.Parse(savegameParts[(int) SfSavegame.DamageMin]);
            _damageMax = int.Parse(savegameParts[(int) SfSavegame.DamageMax]);
            _potionGain = int.Parse(savegameParts[(int) SfSavegame.PotionGain]);
        }

        public SfClass Class
        {
            get { return _class; }
        }

        public int Constitution
        {
            get { return _constitution; }
        }

        public int ConstitutionBonus
        {
            get { return _constitutionBonus; }
        }

        public int ConstitutionBought
        {
            get { return _constitutionBought; }
        }

        public int DamageMax
        {
            get { return _damageMax; }
        }

        public int DamageMin
        {
            get { return _damageMin; }
        }

        public int Dexterity
        {
            get { return _dexterity; }
        }

        public int DexterityBonus
        {
            get { return _dexterityBonus; }
        }

        public int DexterityBought
        {
            get { return _dexterityBought; }
        }

        public int Intelligence
        {
            get { return _intelligence; }
        }

        public int IntelligenceBonus
        {
            get { return _intelligenceBonus; }
        }

        public int IntelligenceBought
        {
            get { return _intelligenceBought; }
        }

        public int Level
        {
            get { return _level; }
        }

        public int Luck
        {
            get { return _luck; }
        }

        public int LuckBonus
        {
            get { return _luckBonus; }
        }

        public int LuckBought
        {
            get { return _luckBought; }
        }

        public int PotionGain
        {
            get { return _potionGain; }
        }

        public int Strength
        {
            get { return _strength; }
        }

        public int StrengthBonus
        {
            get { return _strengthBonus; }
        }

        public int StrengthBought
        {
            get { return _strengthBought; }
        }
    }
}