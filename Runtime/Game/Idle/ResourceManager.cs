using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit.idle
{
    public class ResourceManager
    {
        public ResourceManager()
        {

        }

        private readonly Dictionary<ResourceId, Resource> _resources = new();

        public void RegisterResource(Resource resource)
        {
            if (_resources.ContainsKey(resource.Definition.Identifier))
            {
                Debug.LogError($"Resource already registered: {resource.Definition.DisplayName}");
                return;
            }
            if (resource == null)
            {
                Debug.LogError($"Resource is null: {resource.Definition.DisplayName}");
                return;
            }

            Debug.Log($"Resource registered: {resource.Definition.DisplayName}");
            _resources[resource.Definition.Identifier] = resource;
        }

        public void UnregisterResource(Resource resource)
        {
            if (_resources.ContainsKey(resource.Definition.Identifier))
            {
                Debug.Log($"Resource unregistered: {resource.Definition.DisplayName}");
                _resources.Remove(resource.Definition.Identifier);
            }
            else
            {
                Debug.LogError($"Resource not found: {resource.Definition.DisplayName}");
            }
        }
    }
}
