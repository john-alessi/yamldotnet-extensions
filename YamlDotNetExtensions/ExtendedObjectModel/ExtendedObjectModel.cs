﻿using System.Collections;
using System.Reflection;
using YamlDotNet.Serialization;

namespace YamlDotNetExtensions.ExtendedObjectModel
{
    [Serializable]
    public class ExtendedObjectModel<T> : IDictionary<string, object>
    {
        [YamlIgnore]
        public IDictionary<string, object> UnmatchedProperties = new Dictionary<string, object>();

        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private static readonly Dictionary<string, PropertyInfo> DeserializableProperties =
            typeof(T)
            .GetProperties()
            .Where(p => p.DeclaringType == typeof(T))
            .ToDictionary(p => (p.GetCustomAttribute(typeof(YamlMemberAttribute)) as YamlMemberAttribute)?.Alias ?? p.Name);

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);

            if (DeserializableProperties.ContainsKey(key))
            {
                var propInfo = DeserializableProperties[key];
                propInfo.SetValue(this, value);
            }
            else
            {
                UnmatchedProperties.Add(key, value);
            }
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            foreach (var item in _dictionary)
            {
                array[arrayIndex++] = item;
            }
        }

        public object this[string key]
        {
            get => _dictionary[key];
            set => Add(key, value);
        }

        public void Add(KeyValuePair<string, object> kvp) => Add(kvp.Key, kvp.Value);
        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
        public bool Remove(string key) => _dictionary.Remove(key);
        public bool TryGetValue(string key, out object value) => _dictionary.TryGetValue(key, out value);
        public ICollection<string> Keys => _dictionary.Keys;
        public ICollection<object> Values => _dictionary.Values;
        public void Clear() => _dictionary.Clear();
        public bool Contains(KeyValuePair<string, object> kvp) => _dictionary.Contains(kvp);
        public bool Remove(KeyValuePair<string, object> item) => _dictionary.Remove(item.Key);
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
