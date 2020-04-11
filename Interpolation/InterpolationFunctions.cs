using Character;
using UnityEngine;

namespace Interpolation {
    public static class InterpolationFunctions {
        public static float CubicHermiteSpline(float start, float stop, float m0, float m1, float t) {
            return (2*t*t*t - 3*t*t + 1) * start + (t*t*t - 2*t*t + t)*m0 + (-2*t*t*t + 3*t*t) * stop + (t*t*t - t*t) * m1;
        }



        public static float CubicHermiteSpline3(float p0, float p1, float p2, float dt01, float dt12, float t) {
          /*  if (Mathf.Approximately(p0, p1))
                return InterpolateFloat(p1, p2, t);*/
            
            var m0 = (p1 - p0) / dt01;
            var m1 = (p2 - p1) / dt12;
            if (p0 + 0.01f >= p1 && p1 <= p2 + 0.01f) {
                m0 = 0;
            }

            if (p0 <= p1 + 0.01f && p1 + 0.01f >= p2) {
                m0 = 0;
            }

           /*if (t > 1)
                Debug.LogError("time is " + t);
*/
            var res = CubicHermiteSpline(p1, p2, m0, m1, t);
            // TODO: нормальная формула без min max
         /*   if (p1 <= p2 && res > p2)
                return p2;
            if (p1 >= p2 && res < p2)
                return p2;*/
            return res;
        }


        public static Vector3 Lerp3Points(Vector3 p0, Vector3 p1, Vector3 p2, float dt01, float dt12, float t) {
            float x = CubicHermiteSpline3(p0.x, p1.x, p2.x, dt01, dt12, t);
            float y = CubicHermiteSpline3(p0.y, p1.y, p2.y, dt01, dt12, t);
            float z = CubicHermiteSpline3(p0.z, p1.z, p2.z, dt01, dt12, t);
            return new Vector3(x, y, z);
        }

        public static Vector3 InterpolatePosition(Vector3 lastlastPosition, Vector3 lastPosition, Vector3 nextPosition, 
            float coef) {
            return Lerp3Points(lastlastPosition, lastPosition, nextPosition,
                1, 1,
                coef);/**/ /* * Client.client.interpolationCoef
                    +*/
               /* Vector3.LerpUnclamped(lastPosition, nextPosition, coef);/**/// * (1.0f - Client.client.interpolationCoef)/*)*/;
        }

        public static Quaternion InterpolateRotation(Quaternion lastRotation, Quaternion nextRotation, float coef) {
            return Quaternion.Lerp(lastRotation, nextRotation, coef);
        }

        public static PlayerAnimationState InterpolatePlayerAnimationState(PlayerAnimationState last,
            PlayerAnimationState next, float coef) {
            bool idle = next.idle;//InterpolateBool(last.idle, next.idle, coef);
            float speed = next.speed;//InterpolateFloat(last.speed, next.speed, coef);
            float rotationSpeed = InterpolateFloat(last.speed, next.speed, coef);
            return new PlayerAnimationState() {
                idle = idle,
                speed = speed,
                rotationSpeed = rotationSpeed
            };
        }

        public static bool InterpolateBool(bool last, bool next, float coef) {
            if (last == next) return last;
            if (coef < 0.5f) return last;
            return next;
        }

        public static float InterpolateFloat(float last, float next, float coef) {
            return (next - last) * coef + last;
        }
    }
}