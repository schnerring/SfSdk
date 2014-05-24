using System.Collections.Generic;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     Contains information about a S&amp;F character.
    /// </summary>
    public interface ICharacter : ICharacterBase
    {
        /// <summary>
        ///     The character's strength.
        /// </summary>
        int Strength { get; }

        /// <summary>
        ///     The character's dexterity.
        /// </summary>
        int Dexterity { get; }

        /// <summary>
        ///     The character's intelligence.
        /// </summary>
        int Intelligence { get; }

        /// <summary>
        ///     The character's constitution.
        /// </summary>
        int Constitution { get; }

        /// <summary>
        ///     The character's luck.
        /// </summary>
        int Luck { get; }

        /// <summary>
        ///     The character's defense.
        /// </summary>
        int Defense { get; }

        /// <summary>
        ///     The character's evasion.
        /// </summary>
        int Evasion { get; }

        /// <summary>
        ///     The character's resistance.
        /// </summary>
        int Resistance { get; }

        /// <summary>
        ///     The character's minimum damage.
        /// </summary>
        int DamageMin { get; }

        /// <summary>
        ///     The character's maximum damage.
        /// </summary>
        int DamageMax { get; }

        /// <summary>
        ///     The character's hit points.
        /// </summary>
        int HitPoints { get; }

        /// <summary>
        ///     The character's critical hit chance.
        /// </summary>
        double CriticalHit { get; }

        /// <summary>
        ///     The character's inventory.
        /// </summary>
        IInventory Inventory { get; set; }
    }
}