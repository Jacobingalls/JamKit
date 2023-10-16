using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    public class CanvasGroupOpacityAnimator : MonoBehaviour
    {

        public AnimationCurve Curve;
        public float AnimationTime;
        private float currentTime = 0.0f;
        private float previousValue = -1.0f;

        public CanvasGroup Group;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTime < AnimationTime)
            {
                currentTime = Mathf.MoveTowards(currentTime, AnimationTime, Time.deltaTime);
                if (previousValue != currentTime)
                {
                    Group.alpha = Curve.Evaluate(currentTime);
                }
            }
        }

        public void Reset()
        {
            currentTime = 0;
        }
    }
}
