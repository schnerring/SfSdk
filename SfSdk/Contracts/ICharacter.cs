using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    public interface ICharacter
    {
        string Username { get; }
        int Rank { get; }

        int Strength { get; }
        int Dexterity { get; }
        int Intelligence { get; }
        int Constitution { get; }
        int Luck { get; }
        int Defense { get; }
        int Evasion { get; }
        int Resistance { get; }
        int DamageMin { get; }
        int DamageMax { get; }
        int HitPoints { get; }
        double CriticalHit { get; }

        Task Reresh();
    }
}