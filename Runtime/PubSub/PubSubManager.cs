using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class PubSubManager
    {
        private static PubSubManager _instance;
        public static PubSubManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PubSubManager();
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, HashSet<IPubSubReceivable>> _listeners = new();

        public void Publish(string key, GameObject sender, object value) {
            PubSubListenerEvent e = new PubSubListenerEvent(key, sender, value);
            if (_listeners.ContainsKey(key)) {
                HashSet<IPubSubReceivable> pubSubListeners = _listeners[key];
                foreach (IPubSubReceivable listener in pubSubListeners) {
                    listener.Receive(e);
                }
            }
        }

        public void Subscribe(string key, IPubSubReceivable listener) {
            if (!_listeners.ContainsKey(key)) {
                _listeners[key] = new HashSet<IPubSubReceivable>();
            }

            _listeners[key].Add(listener);
        }

        public void Unsubscribe(string key, IPubSubReceivable listener) {
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