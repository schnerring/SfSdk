namespace SfSdk.ResponseData
{
    internal class CharacterResponse : IResponse
    {
        private readonly string _comment;
        private readonly string _guild;
        private readonly Savegame _savegame;

        internal CharacterResponse(string[] savegameParts, string guild, string comment)
        {
            _savegame = new Savegame(savegameParts);
            _guild = guild;
            _comment = comment;
        }

        public string Comment
        {
            get { return _comment; }
        }

        public string Guild
        {
            get { return _guild; }
        }

        public Savegame Savegame
        {
            get { return _savegame; }
        }
    }
}