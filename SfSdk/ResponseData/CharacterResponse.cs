using System;

namespace SfSdk.ResponseData
{
    internal class CharacterResponse : ResponseBase
    {
        private readonly string _comment;
        private readonly string _guild;
        private readonly Savegame _savegame;

        internal CharacterResponse(string[] args) : base(args)
        {
            if (args.Length < 3) throw new NotImplementedException();

            var savegameParts = ("0/" + args[0]).Split('/');
            _savegame = new Savegame(savegameParts);
            _comment = args[1];
            _guild = args[2];
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