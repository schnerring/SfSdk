using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    internal class Savegame
    {
        private readonly Dictionary<int, string> _savegameDict = new Dictionary<int, string>();

        public Savegame(IList<string> savegameParts)
        {
            for (int i = 0; i < savegameParts.Count; i++)
                _savegameDict.Add(i, savegameParts[i]);
        }

        public T GetValue<T>(int key)
        {
            if (!_savegameDict.ContainsKey(key))
                throw new NotImplementedException("Key is not valiid for savegame data.");
            return _savegameDict[key].Convert<T>();
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