using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class PubSubKVManager
    {

        private static PubSubKVManager _instance;
        public static PubSubKVManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PubSubKVManager();
                }
                return _instance;
            }
        }

        private Dictionary<string, object> state = new();

        private void publishValueForKey(string key, object value)
        {
            PubSubManager.Instance.Publish($"kv.{key}", null, value);
        }

        public void Set(string key, object value)
        {
            state[key] = value;
            publishValueForKey(key, value);
        }

        public object Unset(string key)
        {
            state.Remove(key, out object oldValue);
            publishValueForKey(key, null);
            return oldValue;
        }

        public object Get(string key, object defaultValue)
        {
            return state.GetValueOrDefault(key, defaultValue);
        }
    }
}
