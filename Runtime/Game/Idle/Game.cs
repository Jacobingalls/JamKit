using UnityEngine;

namespace info.jacobingalls.jamkit.idle
{
    public class Game
    {
        private static Game _instance;
        public static Game Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game();
                }
                return _instance;
            }
        }

        public ResourceManager ResourceManager { get; private set; } = new ResourceManager();


        private Game()
        {

        }

        public void Start() {

        }

        public void Update() {

        }
    }
}
