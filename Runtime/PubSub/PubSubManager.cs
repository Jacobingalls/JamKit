using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class PubSubManager : MonoBehaviour
    {
        private static PubSubManager _instance;
        public static PubSubManager Instance
        {
            get
            {
                if (!_instance)
                {
                    var go = new GameObject();
                    go.transform.name = $"Pub-Sub Manager";
                    _instance = go.AddComponent<PubSubManager>();
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, HashSet<PubSubListener>> _listeners = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }

        public void Publish(string key, GameObject sender, object value) {
            PubSubListenerEvent e = new PubSubListenerEvent(key, sender, value);
            if (_listeners.ContainsKey(key)) {
                HashSet<PubSubListener> pubSubListeners = _listeners[key];
                foreach (PubSubListener listener in pubSubListeners) {
                    listener.Receive(e);
                }
            }
        }

        public void Subscribe(string key, PubSubListener listener) {
            if (!_listeners.ContainsKey(key)) {
                _listeners[key] = new HashSet<PubSubListener>();
            }

            _listeners[key].Add(listener);
        }

        public void Unsubscribe(string key, PubSubListener listener) {
            if (!_listeners.ContainsKey(key)) {
                return;
            }

            _listeners[key].Remove(listener);
        }
    }


    public class PubSubListenerEvent {
        public string Key;
        public GameObject sender;
        public object value;

        public PubSubListenerEvent(string key, GameObject sender, object value) {
            this.Key = key;
            this.sender = sender;
            this.value = value;
        }
    }
}