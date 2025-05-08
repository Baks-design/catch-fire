using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace CatchFire
{
    public class CameraZoom
    {
        readonly CharacterData data;
        readonly CinemachineCamera camera;
        readonly float initFOV;
        IEnumerator changeFOVRoutine;
        IEnumerator changeRunFOVRoutine;
        bool running;

        public bool IsZooming { get; private set; }

        public CameraZoom(CharacterData data, CinemachineCamera camera)
        {
            this.data = data;
            this.camera = camera;
            initFOV = camera.Lens.FieldOfView;
        }

        public void ChangeFOV(MonoBehaviour mono)
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

            var currentFOV = camera.Lens.FieldOfView;
            var targetFOV = IsZooming ? initFOV : data.zoomFOV;

            IsZooming = !IsZooming;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var smoothPercent = data.zoomCurve.Evaluate(percent);
                camera.Lens.FieldOfView = Maths.ExpInterp(currentFOV, targetFOV, data.zoomFOVDecay, smoothPercent);
                yield return null;
            }
        }

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

            var currentFOV = camera.Lens.FieldOfView;
            var targetFOV = returning ? initFOV : data.runFOV;

            running = !returning;

            while (percent < 1f)
            {
                percent += Time.deltaTime * speed;
                var smoothPercent = data.runCurve.Evaluate(percent);
                camera.Lens.FieldOfView = Maths.ExpInterp(currentFOV, targetFOV, data.runFOVDecay, smoothPercent);
                yield return null;
            }
        }
    }
}