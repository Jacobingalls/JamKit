using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace info.jacobingalls.jamkit
{

    public abstract class BasePubSubKVListener<T> : MonoBehaviour, IPubSubReceivable
    {

        [Tooltip("Key should not include the 'kv.' namespacing prefix.")]
        public List<PubSubKVListenerDelegateRow<T>> Subscriptions;

        // Start is called before the first frame update
        public virtual void Start()
        {
            Subscribe();
        }

        public virtual void OnDestroy()
        {
            Unsubscribe();
        }

        public virtual void Subscribe()
        {
            foreach (var sub in Subscriptions)
            {
                Subscribe(sub.Key);
                sub.Delegate.Invoke(GetValue(sub));
            }
        }

        public virtual void Subscribe(string key)
        {
            PubSubManager.Instance.Subscribe($"kv.{key}", this);
        }

        public virtual void Subscribe(string key, T defaultValue, UnityEvent<T> e)
        {
            Subscriptions.Add(new PubSubKVListenerDelegateRow<T>(key, defaultValue, e));
            Subscribe(key);
        }

        public virtual void Unsubscribe()
        {
            foreach (var sub in Subscriptions)
            {
                if (PubSubManager.Instance != null)
                {
                    PubSubManager.Instance.Unsubscribe($"kv.{sub.Key}", this);
                }
            }
            Subscriptions.Clear();
        }

        public virtual void Unsubscribe(string key)
        {
            PubSubManager.Instance.Unsubscribe($"kv.{key}", this);
            Subscriptions.Remove(Subscriptions.Find((s) => s.Key == key));
        }

        public virtual void Receive(PubSubListenerEvent e)
        {
            foreach (var sub in Subscriptions)
            {
                if (e.Key  == $"kv.{sub.Key}")
                {
                    Invoke(sub, cast(sub, e.value));
                }
            }
        }

        public virtual void Invoke(PubSubKVListenerDelegateRow<T> sub, object value)
        {
            sub.Delegate.Invoke(cast(sub, value));
        }

        public virtual T GetValue(PubSubKVListenerDelegateRow<T> sub)
        {
            object v = PubSubKVManager.Instance.Get(sub.Key, sub.DefaultValue);
            return cast(sub, v);
        }

        public virtual void SetValue(PubSubKVListenerDelegateRow<T> sub, T value)
        {
            object v = icast(sub, value);
            PubSubKVManager.Instance.Set(sub.Key, v);
        }

        public virtual T cast(PubSubKVListenerDelegateRow<T> sub, object value)
        {
            if (value is T currentValue)
            {
                return currentValue;
            }
            else
            {
                return sub.DefaultValue;
            }
        }

        public virtual object icast(PubSubKVListenerDelegateRow<T> sub, T value)
        {
            return value;
        }

        [Serializable]
        public class PubSubKVListenerDelegateRow<T>
        {
            public string Key;
            public T DefaultValue;
            public UnityEvent<T> Delegate;

            public PubSubKVListenerDelegateRow(string key, T defaultValue, UnityEvent<T> e)
            {
                this.Key = key;
                this.DefaultValue = defaultValue;
                this.Delegate = e;
            }
        }
    }

}
