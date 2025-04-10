using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class DeleteIfWebGL : MonoBehaviour
    {
        void Awake()
        {
            #if UNITY_WEBGL || UNITY_EDITOR
                Destroy(gameObject);
            #endif
        }
    }
}
