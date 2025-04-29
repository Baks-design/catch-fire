using UnityEngine;

namespace CatchFire
{
    public static class Maths
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        public static float ExpDecay(float a, float b, float h, float dt)
        => b + (a - b) * Mathf.Exp(-h * dt);
    }
}