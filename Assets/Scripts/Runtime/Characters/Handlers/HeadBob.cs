using UnityEngine;

namespace CatchFire
{
    public class HeadBob
    {
        readonly HeadBobData headBobData;
        Vector3 finalOffset;
        float xScroll;
        float yScroll;
        bool resetted;
        float currentStateHeight;

        public Vector3 FinalOffset => finalOffset;
        public bool Resetted => resetted;
        public float CurrentStateHeight
        {
            get => currentStateHeight;
            set => currentStateHeight = value;
        }

        public HeadBob(CharacterData data, HeadBobData headBobData)
        {
            this.headBobData = headBobData;

            headBobData.MoveBackwardsFrequencyMultiplier = data.moveBackwardsSpeedPercent;
            headBobData.MoveSideFrequencyMultiplier = data.moveSideSpeedPercent;

            currentStateHeight = 0f;
            xScroll = 0f;
            yScroll = 0f;

            resetted = false;
            finalOffset = Vector3.zero;
        }

        public void ScrollHeadBob(bool running, bool crouching, Vector2 input)
        {
            float amplitudeMultiplier;
            float frequencyMultiplier;
            float additionalMultiplier;
            float xValue;
            float yValue;

            resetted = false;

            amplitudeMultiplier = running ? headBobData.runAmplitudeMultiplier : 1f;
            amplitudeMultiplier = crouching ? headBobData.crouchAmplitudeMultiplier : amplitudeMultiplier;

            frequencyMultiplier = running ? headBobData.runFrequencyMultiplier : 1f;
            frequencyMultiplier = crouching ? headBobData.crouchFrequencyMultiplier : frequencyMultiplier;

            additionalMultiplier = input.y == -1f ? headBobData.MoveBackwardsFrequencyMultiplier : 1f;
            additionalMultiplier = input.x != 0f & input.y == 0f ? headBobData.MoveSideFrequencyMultiplier : additionalMultiplier;

            xScroll += Time.deltaTime * headBobData.xFrequency * frequencyMultiplier;
            yScroll += Time.deltaTime * headBobData.yFrequency * frequencyMultiplier;

            xValue = headBobData.xCurve.Evaluate(xScroll);
            yValue = headBobData.yCurve.Evaluate(yScroll);

            finalOffset.x = xValue * headBobData.xAmplitude * amplitudeMultiplier * additionalMultiplier;
            finalOffset.y = yValue * headBobData.yAmplitude * amplitudeMultiplier * additionalMultiplier;
        }

        public void ResetHeadBob()
        {
            resetted = true;

            xScroll = 0f;
            yScroll = 0f;

            finalOffset = Vector3.zero;
        }
    }
}