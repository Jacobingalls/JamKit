using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class Spin : MonoBehaviour
    {

        public float Speed = 1.0f;

        public Vector3 EulerRotationSpeed = Vector3.one;

        public bool ShouldRotateParent;
        public List<GameObject> objects;


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (ShouldRotateParent) {
                ApplyRotationToObject(gameObject.transform);
            }

            foreach (var obj in objects)
            {
                ApplyRotationToObject(obj.transform);
            }
        }

        void ApplyRotationToObject(Transform transform)
        {
            transform.Rotate(EulerRotationSpeed * Speed * Time.deltaTime);
        }

        public void SetSpeed(float speed)
        {
            this.Speed = speed;
        }
    }
}