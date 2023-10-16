using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace info.jacobingalls.jamkit
{
    public class ImageColorGradientAnimator : MonoBehaviour
    {

        public Gradient Gradient;
        public float AnimationTime;
        private float currentTime = 0.0f;
        private float previousValue = -1.0f;

        public Image Image;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (currentTime < 1)
            {
                currentTime = Mathf.MoveTowards(currentTime, 1, Time.deltaTime / AnimationTime);
                if (previousValue != currentTime)
                {
                    Image.color = Gradient.Evaluate(currentTime);
                }
            }
        }

        public void Reset()
        {
            currentTime = 0;
        }
    }
}
