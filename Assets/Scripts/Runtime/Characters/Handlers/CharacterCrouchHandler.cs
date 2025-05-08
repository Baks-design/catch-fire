using System.Collections;
using UnityEngine;

namespace CatchFire
{
    public class CharacterCrouchHandler
    {
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly Transform yawTransform;
        readonly HeadBob headBob;
        readonly CharacterLandHandler landHandler;
        readonly float initHeight;
        readonly float crouchHeight;
        readonly float crouchStandHeightDifference;
        readonly float initCamHeight;
        readonly float crouchCamHeight;
        Vector3 initCenter;
        Vector3 crouchCenter;
        IEnumerator crouchRoutine;

        public bool IsCrouching { get; private set; }
        public bool IsDuringCrouchAnimation { get; private set; }

        public CharacterCrouchHandler(
            CharacterData data,
            CharacterController controller,
            Transform yawTransform,
            HeadBob headBob,
            CharacterLandHandler landHandler)
        {
            this.data = data;
            this.controller = controller;
            this.yawTransform = yawTransform;
            this.headBob = headBob;
            this.landHandler = landHandler;

            initCenter = controller.center;
            initHeight = controller.height;

            crouchHeight = initHeight * data.crouchPercent;
            crouchCenter = (crouchHeight / 2f + controller.skinWidth) * Vector3.up;

            crouchStandHeightDifference = initHeight - crouchHeight;

            initCamHeight = yawTransform.localPosition.y;
            crouchCamHeight = initCamHeight - crouchStandHeightDifference;
        }

        public void HandleCrouch(
            MonoBehaviour mono, CharacterCollisionController collisionController)
        {
            if (collisionController.GroundChecker.IsGrounded)
                InvokeCrouchRoutine(mono, collisionController.RoofChecker);
        }

        void InvokeCrouchRoutine(MonoBehaviour mono, CharacterRoofChecker roofed)
        {
            if (IsCrouching)
                if (roofed.IsRoofed)
                    return;

            if (landHandler.LandRoutine != null)
                mono.StopCoroutine(landHandler.LandRoutine);

            if (crouchRoutine != null)
                mono.StopCoroutine(crouchRoutine);

            crouchRoutine = CrouchRoutine();
            mono.StartCoroutine(crouchRoutine);
        }

        IEnumerator CrouchRoutine()
        {
            IsDuringCrouchAnimation = true;

            var percent = 0f;
            var speed = 1f / data.crouchTransitionDuration;

            var currentHeight = controller.height;
            var currentCenter = controller.center;

            var desiredHeight = IsCrouching ? initHeight : crouchHeight;
            var desiredCenter = IsCrouching ? initCenter : crouchCenter;

            var camPos = yawTransform.localPosition;
            var camCurrentHeight = camPos.y;
            var camDesiredHeight = IsCrouching ? initCamHeight : crouchCamHeight;

            IsCrouching = !IsCrouching;
            headBob.CurrentStateHeight = IsCrouching ? crouchCamHeight : initCamHeight;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var smoothPercent = data.crouchTransitionCurve.Evaluate(percent);

                controller.height = Maths.ExpDecay(currentHeight, desiredHeight, data.crouchDecay, smoothPercent);
                controller.center = Maths.ExpDecay(currentCenter, desiredCenter, data.crouchDecay, smoothPercent);

                camPos.y = Maths.ExpDecay(camCurrentHeight, camDesiredHeight, data.crouchDecay, smoothPercent);
                yawTransform.localPosition = camPos;

                yield return null;
            }

            IsDuringCrouchAnimation = false;
        }
    }
}