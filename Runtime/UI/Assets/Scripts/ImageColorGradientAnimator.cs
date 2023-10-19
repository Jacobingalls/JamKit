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

        private bool shouldRun = false;
        public bool RunOnLoad = false;

        public bool Loop = false;

        public List<Image> Images;

        private void Awake() {
            shouldRun = RunOnLoad;
        }

        // Update is called once per frame
        void Update()
        {
            if (!shouldRun) { return; }

            currentTime += Time.deltaTime;
            if (currentTime > AnimationTime) {

                // If we are not looping we should saturate.
                if (!Loop) {
                    currentTime = AnimationTime;
                    shouldRun = false;

                // Otherwise we should retain the inpresision of deltaTime to keep a smooth animaiton.
                } else {
                    currentTime -= AnimationTime;
                }
            }

            foreach (var image in Images) {
                image.color = Gradient.Evaluate(currentTime / AnimationTime);
            }
        }

        public void Start() {
            shouldRun = true;
            currentTime = 0;
        }

        public void Stop() {
            shouldRun = false;
            currentTime = AnimationTime;
        }

        public void Resume() {
            shouldRun = true;
        }

        public void Pause() {
            shouldRun = false;
        }
    }
}
