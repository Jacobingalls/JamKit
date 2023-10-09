using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class PubSubSender : MonoBehaviour
    {
        public void Publish(string key) {
            PubSubManager.Instance.Publish(key, gameObject, this);
        }

        public void Publish(string key, object value) {
            PubSubManager.Instance.Publish(key, gameObject, value);
        }
    }
}