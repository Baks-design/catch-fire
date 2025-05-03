using System.Collections.Generic;
using UnityEngine;

namespace CatchFire
{
    public static class WaitFor
    {
        static readonly WaitForFixedUpdate fixedUpdate = new();
        static readonly WaitForEndOfFrame endOfFrame = new();
        static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new(100, new FloatComparer());
        static readonly Dictionary<float, WaitForSecondsRealtime> WaitForRealtimeSecondsDict = new(100, new FloatComparer());

        public static WaitForFixedUpdate FixedUpdate => fixedUpdate;
        public static WaitForEndOfFrame EndOfFrame => endOfFrame;

        public static WaitForSeconds Seconds(float seconds)
        {
            if (seconds < 1f / Application.targetFrameRate)
                return null;

            if (!WaitForSecondsDict.TryGetValue(seconds, out var forSeconds))
            {
                forSeconds = new WaitForSeconds(seconds);
                WaitForSecondsDict[seconds] = forSeconds;
            }

            return forSeconds;
        }

        public static WaitForSecondsRealtime RealtimeSeconds(float seconds)
        {
            if (seconds < 1f / Application.targetFrameRate)
                return null;

            if (!WaitForRealtimeSecondsDict.TryGetValue(seconds, out var forSeconds))
            {
                forSeconds = new WaitForSecondsRealtime(seconds);
                WaitForRealtimeSecondsDict[seconds] = forSeconds;
            }

            return forSeconds;
        }
    }
}