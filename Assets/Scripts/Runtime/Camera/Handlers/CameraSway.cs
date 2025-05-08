using UnityEngine;

namespace CatchFire
{
    public class CameraSway 
    {
        readonly CharacterData data;
        readonly Transform camTransform;
        bool differentDirection;
        float scrollSpeed;
        float xAmountThisFrame;
        float xAmountPreviousFrame;

        public CameraSway(CharacterData data, Transform camTransform)
        {
            this.data = data;
            this.camTransform = camTransform;
        }

        public void ApplySway(Vector3 inputVector, float rawXInput)
        {
            var xAmount = inputVector.x;
            float swayFinalAmount;
            Vector3 swayVector;

            xAmountThisFrame = rawXInput;

            if (rawXInput != 0f)
            {
                if (xAmountThisFrame != xAmountPreviousFrame && xAmountPreviousFrame != 0f)
                    differentDirection = true;

                var speedMultiplier = differentDirection ? data.changeDirectionMultiplier : 1f;
                scrollSpeed += xAmount * data.swaySpeed * Time.deltaTime * speedMultiplier;
            }

            if (xAmountThisFrame == xAmountPreviousFrame)
                differentDirection = false;

            scrollSpeed = Maths.ExpDecay(scrollSpeed, 0f, data.swayDecay, Time.deltaTime * data.returnSpeed);
            scrollSpeed = Mathf.Clamp(scrollSpeed, -1f, 1f);

            if (scrollSpeed < 0f)
                swayFinalAmount = -data.swayCurve.Evaluate(scrollSpeed) * -data.swayAmount;
            else
                swayFinalAmount = data.swayCurve.Evaluate(scrollSpeed) * -data.swayAmount;

            swayVector.z = swayFinalAmount;

            camTransform.localEulerAngles = new Vector3(
                camTransform.localEulerAngles.x,
                camTransform.localEulerAngles.y,
                swayVector.z
            );

            xAmountPreviousFrame = xAmountThisFrame;
        }
    }
}