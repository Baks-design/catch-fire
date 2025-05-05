using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterCameraController : MonoBehaviour
    {
        [SerializeField] CharacterData data;
        [SerializeField] Transform yawTranform;
        [SerializeField] Transform pitchTranform;
        [SerializeField] CinemachineCamera cam;
        IPlayerMapInput inputProvider;
        IEnumerator changeFOVRoutine;
        IEnumerator changeRunFOVRoutine;
        Transform camTransform;
        PerlinNoiseScroller perlinNoiseScroller;
        Vector3 finalRot;
        Vector3 finalPos;
        bool running;
        bool differentDirection;
        float initFOV;
        float yaw;
        float pitch;
        float desiredPitch;
        float desiredYaw;
        float scrollSpeed;
        float xAmountThisFrame;
        float xAmountPreviousFrame;

        public bool IsZooming { get; private set; }

        void Awake()
        {
            InitInputs();
            InitValues();
        }

        void InitInputs()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.LookActionSetup();
            inputProvider.AimActionSetup();
        }

        void InitValues()
        {
            desiredYaw = yaw = yawTranform.eulerAngles.y;
            initFOV = Camera.main.fieldOfView;
            camTransform = Camera.main.transform;
            perlinNoiseScroller = new PerlinNoiseScroller(data.noiseData);
        }

        void OnEnable()
        {
            inputProvider.IsAiming.started += CameraAim;
            inputProvider.IsAiming.performed += CameraAim;
            inputProvider.IsAiming.canceled += CameraAim;
        }

        void OnDisable()
        {
            inputProvider.IsAiming.started -= CameraAim;
            inputProvider.IsAiming.performed -= CameraAim;
            inputProvider.IsAiming.canceled -= CameraAim;
        }

        void CameraAim(InputAction.CallbackContext context)
        {
            if (context.phase is InputActionPhase.Performed ||
                context.phase is InputActionPhase.Canceled)
                ChangeFOV(this);
        }

        void LateUpdate()
        {
            CalculateRotation();
            ApplySmoothRotation();
            ApplyRotation();
            ApplyBreathing();
        }

        #region Movement
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
            yawTranform.eulerAngles = new Vector3(0f, yaw, 0f);
            pitchTranform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
        #endregion

        #region Zoom
        void ChangeFOV(MonoBehaviour mono) 
        {
            if (running)
            {
                IsZooming = !IsZooming;
                return;
            }

            if (changeRunFOVRoutine != null)
                mono.StopCoroutine(changeRunFOVRoutine);

            if (changeFOVRoutine != null)
                mono.StopCoroutine(changeFOVRoutine);

            changeFOVRoutine = ChangeFOVRoutine();
            mono.StartCoroutine(changeFOVRoutine);
        }

        IEnumerator ChangeFOVRoutine()
        {
            var percent = 0f;
            var speed = 1f / data.zoomTransitionDuration;

            var currentFOV = cam.Lens.FieldOfView;
            var targetFOV = IsZooming ? initFOV : data.zoomFOV;

            IsZooming = !IsZooming;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var smoothPercent = data.zoomCurve.Evaluate(percent);
                cam.Lens.FieldOfView = Maths.ExpInterp(currentFOV, targetFOV, data.zoomFOVDecay, smoothPercent);
                yield return null;
            }
        }

        public void ChangeRunFOV(bool returning) => ChangeRunFOV(returning, this);

        public void ChangeRunFOV(bool returning, MonoBehaviour mono)
        {
            if (changeFOVRoutine != null)
                mono.StopCoroutine(changeFOVRoutine);

            if (changeRunFOVRoutine != null)
                mono.StopCoroutine(changeRunFOVRoutine);

            changeRunFOVRoutine = ChangeRunFOVRoutine(returning);
            mono.StartCoroutine(changeRunFOVRoutine);
        }

        IEnumerator ChangeRunFOVRoutine(bool returning)
        {
            var percent = 0f;
            var duration = returning ? data.runReturnTransitionDuration : data.runTransitionDuration;
            var speed = 1f / duration;

            var currentFOV = cam.Lens.FieldOfView;
            var targetFOV = returning ? initFOV : data.runFOV;

            running = !returning;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var smoothPercent = data.runCurve.Evaluate(percent);
                cam.Lens.FieldOfView = Maths.ExpInterp(currentFOV, targetFOV, data.runFOVDecay, smoothPercent);
                yield return null;
            }
        }
        #endregion

        #region Sway
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
        #endregion

        #region Breathing
        void ApplyBreathing() //FIXME
        {
            perlinNoiseScroller.UpdateNoise();

            var posOffset = Vector3.zero;
            var rotOffset = Vector3.zero;

            switch (data.noiseData.transformTarget)
            {
                case TransformTarget.Position:
                    {
                        if (data.x)
                            posOffset.x += perlinNoiseScroller.Noise.x;
                        if (data.y)
                            posOffset.y += perlinNoiseScroller.Noise.y;
                        if (data.z)
                            posOffset.z += perlinNoiseScroller.Noise.z;

                        finalPos.x = data.x ? posOffset.x : transform.localPosition.x;
                        finalPos.y = data.y ? posOffset.y : transform.localPosition.y;
                        finalPos.z = data.z ? posOffset.z : transform.localPosition.z;

                        transform.localPosition = finalPos;
                        break;
                    }
                case TransformTarget.Rotation:
                    {
                        if (data.x)
                            rotOffset.x += perlinNoiseScroller.Noise.x;
                        if (data.y)
                            rotOffset.y += perlinNoiseScroller.Noise.y;
                        if (data.z)
                            rotOffset.z += perlinNoiseScroller.Noise.z;

                        finalRot.x = data.x ? rotOffset.x : transform.localEulerAngles.x;
                        finalRot.y = data.y ? rotOffset.y : transform.localEulerAngles.y;
                        finalRot.z = data.z ? rotOffset.z : transform.localEulerAngles.z;

                        transform.localEulerAngles = finalRot;

                        break;
                    }
                case TransformTarget.Both:
                    {
                        if (data.x)
                        {
                            posOffset.x += perlinNoiseScroller.Noise.x;
                            rotOffset.x += perlinNoiseScroller.Noise.x;
                        }
                        if (data.y)
                        {
                            posOffset.y += perlinNoiseScroller.Noise.y;
                            rotOffset.y += perlinNoiseScroller.Noise.y;
                        }
                        if (data.z)
                        {
                            posOffset.z += perlinNoiseScroller.Noise.z;
                            rotOffset.z += perlinNoiseScroller.Noise.z;
                        }

                        finalPos.x = data.x ? posOffset.x : transform.localPosition.x;
                        finalPos.y = data.y ? posOffset.y : transform.localPosition.y;
                        finalPos.z = data.z ? posOffset.z : transform.localPosition.z;

                        finalRot.x = data.x ? rotOffset.x : transform.localEulerAngles.x;
                        finalRot.y = data.y ? rotOffset.y : transform.localEulerAngles.y;
                        finalRot.z = data.z ? rotOffset.z : transform.localEulerAngles.z;

                        transform.localPosition = finalPos;
                        transform.localEulerAngles = finalRot;

                        break;
                    }
            }
        }
        #endregion
    }
}