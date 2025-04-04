using UnityEngine;
using info.jacobingalls.jamkit.idle;

namespace info.jacobingalls.jamkit
{
    public class IdleGameManager : MonoBehaviour
    {
        void Start()
        {
            foreach (var resource in GameResources.Resources)
            {
                Game.Instance.ResourceManager.RegisterResource(resource);
            }

            Game.Instance.Start();
        }

        void Update()
        {
            Game.Instance.Update();
        }
    }
}
