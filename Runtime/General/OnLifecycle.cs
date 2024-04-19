using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace info.jacobingalls.jamkit
{
    public class OnLifecycle : MonoBehaviour
    {

        public UnityEvent<GameObject> onAwake;
        public UnityEvent<GameObject> onStart;
        public UnityEvent<GameObject> onUpdate;
        public UnityEvent<GameObject> onDestroy;

        private void Awake()
        {
            onAwake.Invoke(gameObject);
        }

        private void Start()
        {
            onStart.Invoke(gameObject);
        }

        private void Update()
        {
            onUpdate.Invoke(gameObject);
        }

        private void OnDestroy()
        {
            onDestroy.Invoke(gameObject);
        }
    }
}
