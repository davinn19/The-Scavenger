using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs, implement
    [Serializable]
    public class PersistentData : ISerializationCallbackReceiver
    {
        [SerializeField] private string serializedData;

        private readonly Dictionary<string, string> data = new();


        public void Add<T>(string key, T value)
        {
            data.Add(key, value.ToString());
        }

        public string GetValue(string key) => data[key];
        public int GetInt(string key) => int.Parse(GetValue(key));
        public bool GetBool(string key) => bool.Parse(GetValue(key));
        public float GetFloat(string key) => float.Parse(GetValue(key));


        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void Clear()
        {
            serializedData = "";
            data.Clear();
        }

        public void OnBeforeSerialize()
        {
            serializedData = "";
            foreach(KeyValuePair<string, string> kvp in data)
            {
                serializedData += kvp.Key + "=" + kvp.Value + "|";
            }
        }

        public void OnAfterDeserialize()
        {
            data.Clear();

            string[] pairs = serializedData.Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('=');
                string key = parts[0];
                string value = parts[1];
                data.Add(key, value);
            }
        }
    }

}
