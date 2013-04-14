using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    /// <summary>
    ///     The savegame which is attatched to every character in S&amp;F.
    /// </summary>
    internal class Savegame
    {
        private readonly Dictionary<int, string> _savegameDict = new Dictionary<int, string>();

        /// <summary>
        ///     Creates a new savegame.
        /// </summary>
        /// <param name="savegameParts">A array of type <see cref="string" /> containing the savegame parts.</param>
        public Savegame(string[] savegameParts)
        {
            for (int i = 0; i < savegameParts.Length; i++)
                _savegameDict.Add(i, savegameParts[i]);
        }

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <typeparam name="T">The expected type of the corresponding value.</typeparam>
        /// <param name="key">A key of type <see cref="int" />.</param>
        /// <returns>The corresponding value for the given key as T.</returns>
        public T GetValue<T>(int key)
        {
            if (!_savegameDict.ContainsKey(key))
                throw new NotImplementedException("Key is not valiid for savegame data.");
            return _savegameDict[key].Convert<T>();
        }

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <typeparam name="T">The expected type of the corresponding value.</typeparam>
        /// <param name="key">A key of type <see cref="SF" /> starting with "Sg".</param>
        /// <returns>The corresponding value for the given key as T.</returns>
        public T GetValue<T>(SF key)
        {
            return GetValue<T>((int) key);
        }

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <param name="key">A key of type <see cref="int" />.</param>
        /// <returns>The corresponding value for the given key as <see cref="int" />.</returns>
        public int GetValue(int key)
        {
            return GetValue<int>(key);
        }

        /// <summary>
        ///     Tries to get a value from the savegame given on a key.
        /// </summary>
        /// <param name="key">A key of type <see cref="SF" /> starting with "Sg".</param>
        /// <returns>The corresponding value for the given key as <see cref="int" />.</returns>
        public int GetValue(SF key)
        {
            return GetValue<int>((int) key);
        }
    }
}