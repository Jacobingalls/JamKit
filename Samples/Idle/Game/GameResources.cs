using info.jacobingalls.jamkit.idle;
using System.Collections.Generic;

namespace info.jacobingalls.jamkit
{
    public static class GameResourceIds
    {
        public static ResourceId Cactus { get { return new ResourceId("Cactus"); } }
        public static ResourceId Flower { get { return new ResourceId("Flower"); } }
        public static ResourceId Fruit { get { return new ResourceId("Fruit"); } }
    }

    public static class GameResources
    {
        private static List<Resource> _resources;
        public static List<Resource> Resources
        {
            get
            {
                if (_resources != null)
                {
                    return _resources;
                }
                _resources = new List<Resource>
                {
                    CreateCactusResource(),
                    CreateFlowerResource(),
                    CreateFruitResource()
                };

                return _resources;
            }
        }
        private static Resource CreateCactusResource()
        {
            var resource = new ResourceDefinition(GameResourceIds.Cactus, "Cactus");
            var cactus = new Resource(resource, 0f);
            return cactus;
        }

        private static Resource CreateFlowerResource()
        {
            var resource = new ResourceDefinition(GameResourceIds.Flower, "Flower");
            var flower = new Resource(resource, 0f);
            return flower;
        }

        private static Resource CreateFruitResource()
        {
            var resource = new ResourceDefinition(GameResourceIds.Fruit, "Fruit");
            var fruit = new Resource(resource, 0f);
            return fruit;
        }
        
    }
}
