using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

namespace info.jacobingalls.jamkit
{

    public interface IPubSubReceivable
    {
        public void Receive(PubSubListenerEvent e);
    }

    public class PubSubListener : MonoBehaviour, IPubSubReceivable
    {
        public List<PubSubListenerDelegateRow> Subscriptions;

        // Start is called before the first frame update
        void Start()
        {
            Subscribe();
        }

        void OnDestroy() {
            Unsubscribe();
        }

        public void Subscribe() {
            foreach (var sub in Subscriptions) {
                Subscribe(sub.Key);
            }
        }

        public void Subscribe(string key) {
            PubSubManager.Instance.Subscribe(key, this);
        }

        public void Subscribe(string key, PubSubListenerUnityEvent e) {
            Subscriptions.Add(new PubSubListenerDelegateRow(key, e));
            Subscribe(key);
        }

        public void Unsubscribe() {
            foreach (var sub in Subscriptions) {
                if (PubSubManager.Instance != null)
                {
                    PubSubManager.Instance.Unsubscribe(sub.Key, this);
                }
            }
            Subscriptions.Clear();
        }

        public void Unsubscribe(string key) {
            PubSubManager.Instance.Unsubscribe(key, this);
            Subscriptions.Remove(Subscriptions.Find((s) => s.Key == key));
        }

        public void Receive(PubSubListenerEvent e) {
            foreach (var sub in Subscriptions) {
                if (sub.Key == e.Key) {
                    sub.Delegate.Invoke(e);
                }
            }
        }
    }

    [Serializable]
    public class PubSubListenerUnityEvent: UnityEvent<PubSubListenerEvent> { }

    [Serializable]
    public class PubSubListenerDelegateRow {
        public string Key;
        public PubSubListenerUnityEvent Delegate;

        public PubSubListenerDelegateRow(string key, PubSubListenerUnityEvent e) {
            this.Key = key;
            this.Delegate = e;
        }
    }
}