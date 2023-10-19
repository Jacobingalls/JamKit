using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace info.jacobingalls.jamkit
{
    public class EnemyFocusAnimationController : MonoBehaviour
    {

        public VerticalLayoutGroup VerticalLayoutGroup;
        public float ClosedSpacing, OpenSpacing;
        public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);

        private float previousTime = -1f;
        private float currentTime = 0f;
        public float AnimationTime = 1f;

        public bool Focused = false;
        public bool StartFocused = false;

        public List<CanvasGroup> CanvasGroups;

        public float HealthBarSizeClosed, HealthBarSizeOpen;
        public float HealthBarWidthClosed, HealthBarWidthOpen;
        public LayoutElement HealthBarLayout;

        void Awake()
        {
            Focused = StartFocused;
        }

        // Update is called once per frame
        void Update()
        {
            if (Focused) {
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(currentTime, AnimationTime);
            } else {
                currentTime -= Time.deltaTime;
                currentTime = Mathf.Max(currentTime, 0);
            }

            if (previousTime == currentTime) { return; }
            previousTime = currentTime;

            var progress = AnimationCurve.Evaluate(currentTime / AnimationTime);
            VerticalLayoutGroup.spacing = Mathf.Lerp(ClosedSpacing, OpenSpacing, progress);

            foreach (var CanvasGroup in CanvasGroups) {
                CanvasGroup.alpha = progress;
            }

            var healthBarPreferredHeight = Mathf.Lerp(HealthBarSizeClosed, HealthBarSizeOpen, progress);
            var healthBarPreferredWidth = Mathf.Lerp(HealthBarWidthClosed, HealthBarWidthOpen, progress);
            HealthBarLayout.preferredHeight = healthBarPreferredHeight;
            HealthBarLayout.preferredWidth = healthBarPreferredWidth;
        }

        public void ToggleFocus() {
            Focused = !Focused;
        }
    }
}
