using UnityEngine;

namespace info.jacobingalls.jamkit
{

    public static class ExponentialDecay
    {

        // https://acegikmo.substack.com/p/lerp-smoothing-is-broken
        public static float Evaluate(float a, float b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }

        public static float DecayTowards(this float a, float b, float decay, float deltaTime)
        {
            return Evaluate(a, b, decay, deltaTime);
        }

        public static Vector3 Evaluate(Vector3 a, Vector3 b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }

        public static Vector3 DecayTowards(this Vector3 a, Vector3 b, float decay, float deltaTime)
        {
            return Evaluate(a, b, decay, deltaTime);
        }

        public static Vector2 Evaluate(Vector2 a, Vector2 b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }

        public static Vector2 DecayTowards(this Vector2 a, Vector2 b, float decay, float deltaTime)
        {
            return Evaluate(a, b, decay, deltaTime);
        }

        public static Quaternion Evaluate(Quaternion a, Quaternion b, float decay, float deltaTime)
        {
            return Quaternion.Slerp(b, a, Mathf.Exp(-decay * deltaTime));
        }

        public static Quaternion DecayTowards(this Quaternion a, Quaternion b, float decay, float deltaTime)
        {
            return Evaluate(a, b, decay, deltaTime);
        }

        public static Color Evaluate(Color a, Color b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }

        public static Color DecayTowards(this Color a, Color b, float decay, float deltaTime)
        {
            return Evaluate(a, b, decay, deltaTime);
        }

        public static Transform PositionDecayTowards(this Transform transform, Vector3 targetPosition, float decay, float deltaTime)
        {
            transform.position = transform.position.DecayTowards(targetPosition, decay, deltaTime);
            return transform;
        }

        public static Transform LocalPositionDecayTowards(this Transform transform, Vector3 targetPosition, float decay, float deltaTime)
        {
            transform.localPosition = transform.localPosition.DecayTowards(targetPosition, decay, deltaTime);
            return transform;
        }

        public static Transform LocalScaleDecayTowards(this Transform transform, Vector3 targetScale, float decay, float deltaTime)
        {
            transform.localScale = transform.localScale.DecayTowards(targetScale, decay, deltaTime);
            return transform;
        }

        public static Transform RotationDecayTowards(this Transform transform, Quaternion targetRotation, float decay, float deltaTime)
        {
            transform.rotation = transform.rotation.DecayTowards(targetRotation, decay, deltaTime);
            return transform;
        }

        public static Transform LocalRotationDecayTowards(this Transform transform, Quaternion targetRotation, float decay, float deltaTime)
        {
            transform.localRotation = transform.localRotation.DecayTowards(targetRotation, decay, deltaTime);
            return transform;
        }
    }
}