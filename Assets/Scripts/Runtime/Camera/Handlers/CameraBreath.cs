using UnityEngine;

namespace CatchFire
{
    public class CameraBreath 
    {
        readonly CharacterData data;
        readonly Transform transform;
        readonly PerlinNoiseScroller perlinNoiseScroller;
        Vector3 finalRot;
        Vector3 finalPos;

        public CameraBreath(CharacterData data, Transform transform)
        {
            this.data = data;
            this.transform = transform;
            perlinNoiseScroller = new PerlinNoiseScroller(data.noiseData);
        }

        public void ApplyBreathing()
        {
            //if() condition

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
    }
}