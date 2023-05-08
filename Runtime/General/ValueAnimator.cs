using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace info.jacobingalls.jamkit
{
    public class ValueAnimator : MonoBehaviour
    {

        public AnimationCurve ValueCurve;

        public float Scaler = 1.0f; 

        public UnityEvent<float> ValuesToSet;

        public float Speed = 1.0f;

        public float TotalTime = 1.0f;

        public float CurrentTime = 0;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            CurrentTime += Speed * Time.deltaTime;
            if (CurrentTime > TotalTime) {
                CurrentTime -= TotalTime;
            }

            ValuesToSet.Invoke(Scaler * ValueCurve.Evaluate(CurrentTime));
        }
    }
}
