using UnityEngine;

namespace CatchFire
{
    public static class Maths
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            angle %= 360f;

            if (angle < -180f) angle += 360f;
            if (angle > 180f) angle -= 360f;

            return Mathf.Clamp(angle, min, max);
        }

        public static float ExpDecay(float a, float b, float h, float dt)
        => b + (a - b) * Mathf.Exp(-h * dt);

        public static Vector2 ExpDecay(Vector2 a, Vector2 b, float h, float dt)
        => b + (a - b) * Mathf.Exp(-h * dt);

        public static Vector3 ExpDecay(Vector3 a, Vector3 b, float h, float dt)
        => b + (a - b) * Mathf.Exp(-h * dt);

        public static Quaternion ExpDecay(Quaternion a, Quaternion b, float h, float dt)
        => Quaternion.Slerp(a, b, 1f - Mathf.Exp(-h * dt));

        public static float InvExpDecay(float current, float start, float end, float halfLife)
        {
            if (Mathf.Approximately(start, end)) return 0f;
            if (Mathf.Approximately(current, start)) return 0f;
            if (Mathf.Approximately(current, end)) return 1f;

            if ((current > start && current > end) || (current < start && current < end))
                return current > start ? 1f : 0f;

            var progress = Mathf.Log((current - end) / (start - end)) / -halfLife;
            return Mathf.Clamp01(progress);
        }

        public static float ExpInterp(float current, float target, float speed, float deltaTime)
        => current + (target - current) * (1f - Mathf.Exp(-speed * deltaTime));
    }
}