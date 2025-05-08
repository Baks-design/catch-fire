using System.Collections;
using UnityEngine;

namespace CatchFire
{
    public class CharacterLandHandler 
    {
        readonly CharacterData data;
        readonly Transform yawTransform;
        IEnumerator landRoutine;

        public IEnumerator LandRoutine => landRoutine;

        public CharacterLandHandler(
            CharacterData data,
            Transform yawTransform)
        {
            this.data = data;
            this.yawTransform = yawTransform;
        }

        public void HandleLanding(
            MonoBehaviour mono, CharacterGroundChecker grounded, CharacterGravityHandler gravity)
        {
            if (!grounded.IsLanded)
                InvokeLandingRoutine(mono, gravity);
        }

        void InvokeLandingRoutine(MonoBehaviour mono, CharacterGravityHandler gravity)
        {
            if (landRoutine != null)
                mono.StopCoroutine(landRoutine);

            landRoutine = LandingRoutine(gravity);
            mono.StartCoroutine(landRoutine);
        }

        IEnumerator LandingRoutine(CharacterGravityHandler gravity)
        {
            var percent = 0f;
            var speed = 1f / data.landDuration;

            var localPos = yawTransform.localPosition;
            var initLandHeight = localPos.y;

            var landAmount = gravity.InAirTimer > data.landTimer ? data.highLandAmount : data.lowLandAmount;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var desiredY = data.landCurve.Evaluate(percent) * landAmount;

                localPos.y = initLandHeight + desiredY;
                yawTransform.localPosition = localPos;

                yield return null;
            }
        }
    }
}