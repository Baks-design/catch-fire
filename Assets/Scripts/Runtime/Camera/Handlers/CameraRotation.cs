using UnityEngine;

namespace CatchFire
{
    public class CameraRotation 
    {
        readonly CharacterData data;
        readonly IPlayerMapInput inputProvider;
        readonly Transform yawTransform;
        readonly Transform pitchTransform;
        float yaw;
        float pitch;
        float desiredPitch;
        float desiredYaw;

        public CameraRotation(
            CharacterData data,
            IPlayerMapInput inputProvider,
            Transform yawTransform,
            Transform pitchTransform)
        {
            this.data = data;
            this.inputProvider = inputProvider;
            this.yawTransform = yawTransform;
            this.pitchTransform = pitchTransform;

            desiredYaw = yaw = yawTransform.eulerAngles.y;
        }

        public void HandleRotation()
        {
            CalculateRotation();
            ApplySmoothRotation();
            ApplyRotation();
        }

        void CalculateRotation()
        {
            desiredYaw += inputProvider.LookInput.x * Time.deltaTime * data.smoothAmount;

            desiredPitch += inputProvider.LookInput.y * Time.deltaTime * data.smoothAmount;
            desiredPitch = Maths.ClampAngle(desiredPitch, data.bottomClamp, data.topClamp);
        }

        void ApplySmoothRotation()
        {
            yaw = Maths.ExpDecay(yaw, desiredYaw, data.cameraSpeedDecay, data.smoothAmount * Time.deltaTime);
            pitch = Maths.ExpDecay(pitch, desiredPitch, data.cameraSpeedDecay, data.smoothAmount * Time.deltaTime);
        }

        void ApplyRotation()
        {
            yawTransform.eulerAngles = new Vector3(0f, yaw, 0f);
            pitchTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }
}