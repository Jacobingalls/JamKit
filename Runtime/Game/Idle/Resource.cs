using UnityEngine;

namespace info.jacobingalls.jamkit.idle
{
    public class ResourceId
    {
        public ResourceId(string value) { Value = value; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (ResourceId)obj;
            
            return other.Value == Value;
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public struct ResourceDefinition
    {
        public ResourceId Identifier { get; }
        public string DisplayName { get; set; }

        public ResourceDefinition(ResourceId id, string displayName)
        {
            Identifier = id;
            DisplayName = displayName;
        }
    }

    public class Resource
    {


        public ResourceDefinition Definition { get; }

        private float _amount;
        public float Amount
        {
            get { return _amount; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > MaxCapacity)
                {
                    value = MaxCapacity;
                }
                _amount = value;
            }
        }

        public float MaxCapacity { get; private set; }

        public Resource(ResourceDefinition definition, float initialAmount = 0f)
        {
            Definition = definition;
            Amount = initialAmount;
            MaxCapacity = 100;
        }
    }
}
