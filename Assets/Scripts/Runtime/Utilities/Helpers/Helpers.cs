using UnityEngine;

namespace CatchFire
{
    public static class Helpers
    {
        public static WaitForSeconds GetWaitForSeconds(float seconds) => WaitFor.Seconds(seconds);
        public static WaitForSecondsRealtime GetWaitForRealtimeSeconds(float seconds) => WaitFor.RealtimeSeconds(seconds);
    }
}