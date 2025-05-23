﻿using UnityEngine;

namespace Digger.Modules.AdvancedOperations.Splines
{
    public class SplineWalker : MonoBehaviour
    {
        public BezierSpline spline;

        public float duration;

        public bool lookForward;

        public SplineWalkerMode mode;
        private bool goingForward = true;

        private float progress;

        private void Update()
        {
            if (goingForward) {
                progress += Time.deltaTime / duration;
                if (progress > 1f) {
                    if (mode == SplineWalkerMode.Once) {
                        progress = 1f;
                    } else if (mode == SplineWalkerMode.Loop) {
                        progress -= 1f;
                    } else {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            } else {
                progress -= Time.deltaTime / duration;
                if (progress < 0f) {
                    progress = -progress;
                    goingForward = true;
                }
            }

            var position = spline.GetPoint(progress);
            transform.localPosition = position;
            if (lookForward) transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}