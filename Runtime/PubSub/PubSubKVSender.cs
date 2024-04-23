using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{

    public class PubSubKVSender : MonoBehaviour
    {

        [Tooltip("Should not include the 'kv.' namespacing prefix.")]
        public string Key;

        public void SetString(string value) { Set(value); }
        public void SetInt(int value) { Set(value); }
        public void SetFloat(float value) { Set(value); }
        public void SetGameObject(GameObject value) { Set(value); }
        public void SetSprite(Sprite value) { Set(value); }
        public void SetBool(bool value) { Set(value); }

        public void AddInt(int value)
        {
            int v = (int)PubSubKVManager.Instance.Get(Key, 0);
            Set(v + value);
        }

        public void AddFloat(float value)
        {
            float v = (float)PubSubKVManager.Instance.Get(Key, 0);
            Set(v + value);
        }

        public void MultiplyInt(int value)
        {
            int v = (int)PubSubKVManager.Instance.Get(Key, 0);
            Set(v * value);
        }

        public void MultiplyFloat(float value)
        {
            float v = (float)PubSubKVManager.Instance.Get(Key, 0);
            Set(v * value);
        }

        public void Toggle()
        {
            bool v = (bool)PubSubKVManager.Instance.Get(Key, false);
            Set(!v);
        }

        public void Set(object value)
        {
            PubSubKVManager.Instance.Set(Key, value);
        }

        public void Unset()
        {
            PubSubKVManager.Instance.Unset(Key);
        }
    }
}
