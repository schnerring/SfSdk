using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SfBot
{
    /// <summary>
    ///     This class is implemented to store user settings in an Isolated storage file.
    ///     from http://f10andf11.blogspot.de/2012/03/wpf-implement-isolatedstoragesettings.html
    /// </summary>
    public class IsolatedStorageSettings : IDictionary<string, Object>
    {
        #region Constants/Variables

        private const string Filename = "Settings.bin";
        private static readonly Dictionary<string, object> AppDictionary = new Dictionary<string, object>();

        private static readonly IsolatedStorageSettings StaticIsolatedStorageSettings =
            new IsolatedStorageSettings();

        private static readonly IFormatter Formatter = new BinaryFormatter();

        #endregion

        #region Singleton Implementation

        /// <summary>
        ///     Its static constructor.
        /// </summary>
        static IsolatedStorageSettings()
        {
            LoadData();
        }

        /// <summary>
        ///     Its a private constructor.
        /// </summary>
        private IsolatedStorageSettings()
        {
        }

        /// <summary>
        ///     Its a static singleton instance.
        /// </summary>
        public static IsolatedStorageSettings Instance
        {
            get { return StaticIsolatedStorageSettings; }
        }

        // public acces´s for tests
        public static void LoadData()
        {
            // IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForAssembly();
            if (isoStore.GetFileNames(Filename).Length == 0)
            {
                // File not exists. Let us NOT try to DeSerialize it.        
                return;
            }

            // Read the stream from Isolated Storage.    
            Stream stream = new IsolatedStorageFileStream(Filename, FileMode.OpenOrCreate, isoStore);
            if (stream != null)
            {
                try
                {
                    // DeSerialize the Dictionary from stream.    
                    object bytes = Formatter.Deserialize(stream);

                    var appData = (Dictionary<string, object>) bytes;

                    // Enumerate through the collection and load our Dictionary.            
                    IDictionaryEnumerator enumerator = appData.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        AppDictionary[enumerator.Key.ToString()] = enumerator.Value;
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     It Checks if Dictionary object has item corresponding to passed key,
        ///     if True then it returns that object else it returns default value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public object this[string key, Object defaultvalue]
        {
            get
            {
                if (AppDictionary.ContainsKey(key))
                {
                    return AppDictionary[key];
                }
                else
                {
                    return defaultvalue;
                }
            }
            set
            {
                AppDictionary[key] = value;
                Save();
            }
        }

        /// <summary>
        ///     It serializes dictionary in binary format and stores it in a binary file.
        /// </summary>
        public void Save()
        {
            // IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForAssembly();

            Stream stream = new IsolatedStorageFileStream(Filename, FileMode.Create, isoStore);
            if (stream != null)
            {
                try
                {
                    // Serialize dictionary into the IsolatedStorage.                                
                    Formatter.Serialize(stream, AppDictionary);
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        #endregion

        #region IDictionary<string, object> Members

        public void Add(string key, object value)
        {
            AppDictionary.Add(key, value);
            Save();
        }

        public bool ContainsKey(string key)
        {
            return AppDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return AppDictionary.Keys; }
        }

        public bool Remove(string key)
        {
            try
            {
                Save();
                AppDictionary.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out object value)
        {
            return AppDictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return AppDictionary.Values; }
        }

        public object this[string key]
        {
            get { return AppDictionary[key]; }
            set
            {
                AppDictionary[key] = value;
                Save();
            }
        }


        public void Add(KeyValuePair<string, object> item)
        {
            AppDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            AppDictionary.Clear();
            Save();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return AppDictionary.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return AppDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return AppDictionary.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return AppDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AppDictionary.GetEnumerator();
        }

        #endregion
    }
}