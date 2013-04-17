using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     The savegame which is attatched to every character in S&amp;F.
    /// </summary>
    internal interface ISavegame
    {
        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <typeparam name="T">The expected type of the corresponding value.</typeparam>
        /// <param name="key">A key of type <see cref="int" />.</param>
        /// <returns>The corresponding value for the given key as T.</returns>
        T GetValue<T>(int key);

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <typeparam name="T">The expected type of the corresponding value.</typeparam>
        /// <param name="key">A key of type <see cref="SF" /> starting with "Sg".</param>
        /// <returns>The corresponding value for the given key as T.</returns>
        T GetValue<T>(SF key);

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <param name="key">A key of type <see cref="int" />.</param>
        /// <returns>The corresponding value for the given key as <see cref="int" />.</returns>
        int GetValue(int key);

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <param name="key">A key of type <see cref="SF" /> starting with "Sg".</param>
        /// <returns>The corresponding value for the given key as <see cref="int" />.</returns>
        int GetValue(SF key);
    }

    /// <summary>
    ///     The savegame which is attatched to every character in S&amp;F.
    /// </summary>
    internal class Savegame : ISavegame
    {
        private readonly Dictionary<int, string> _savegameDict = new Dictionary<int, string>();

        /// <summary>
        ///     Creates a new savegame.
        /// </summary>
        /// <param name="savegameString">A <see cref="string" /> containing the savegame parts.</param>
        public Savegame(string savegameString)
        {
            var savegameParts = ("0/" + savegameString).Split('/');
            if (savegameParts.Length <= (int) SF.SgServerTime)
                throw new ArgumentException("The savegame string is not valid not long enough.",
                                            "savegameString");

            for (int i = 0; i < savegameParts.Length; i++)
                _savegameDict.Add(i, savegameParts[i]);
        }

        public T GetValue<T>(int key)
        {
            return !_savegameDict.ContainsKey(key)
                ? default(T)
                : _savegameDict[key].Convert<T>();
        }

        public T GetValue<T>(SF key)
        {
            return GetValue<T>((int) key);
        }

        public int GetValue(int key)
        {
            return GetValue<int>(key);
        }

        public int GetValue(SF key)
        {
            return GetValue<int>((int) key);
        }
    }
}