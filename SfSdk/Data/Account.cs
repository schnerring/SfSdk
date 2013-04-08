namespace SfSdk.Data
{
    internal class Account : IAccount
    {
        public int Gold { get; private set; }
        public int Silver    { get; private set; }
        public int Mushrooms { get; private set; }

        internal Account(int mushrooms, int gold, int silver)
        {
            Mushrooms = mushrooms;
            Gold = gold;
            Silver = silver;
        }
    }
}