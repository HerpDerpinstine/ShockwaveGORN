using UnityEngine;

namespace ShockwaveGORN
{
    internal static class Extensions
    {
        private static CapsuleCollider collider;
        internal static void GetBodyForwardPos(this Player _this, out Vector3 centerPosition, out (Vector3, Vector3, Vector3) directions, out (float, float) size)
        {
            if (collider == null)
                collider = _this.damageRelay.transform.GetComponent<CapsuleCollider>();
            var oldposition = collider.transform.localPosition;

            size = (collider.bounds.size.x, collider.bounds.size.y);

            centerPosition = 
                collider.transform.localPosition = 
                    oldposition
                    + collider.center;

            directions = (
                collider.transform.forward,
                collider.transform.right,
                collider.transform.up);

            collider.transform.localPosition = oldposition;
        }

        internal static Vector3 CenterBodyPosToTorso(this Vector3 _this, Vector3 up, float height)
            => _this + (up * (height / 4));

        internal static (Vector3, Vector3) GetHapticBounds(this Vector3 _this, (Vector3, Vector3, Vector3) directions, (float, float) size, bool backwards = false)
        {
            Vector3 forwardMove = (directions.Item1 * (size.Item1 / 2));
            Vector3 rightMove = (directions.Item2 * (size.Item1 / 2));
            Vector3 upMove = (directions.Item3 * (size.Item2 / 2));

            Vector3 centerPos = backwards ? (_this - forwardMove) : (_this + forwardMove);
            
            Vector3 topCenterPos = centerPos + upMove;
            Vector3 bottomCenterPos = centerPos - upMove;

            return ((topCenterPos - rightMove), (bottomCenterPos + rightMove));
        }

        internal static Vector3 GetPositionFromPoint(this (Vector3, Vector3) _this, (float, float) point)
        {
            Vector3 distance = _this.Item2 - _this.Item1;
            distance.x *= 1f - point.Item1;
            distance.y *= point.Item2;
            return _this.Item1 + distance;
        }

        internal static float ScaleIntensity(this float _this)
            => (200 * _this) / 100;

        internal static (float, float) GetPositionAngle(this Vector3 _this, Vector3 targetPos, Vector3 targetForward)
        {
            var targetDir = _this - targetPos;
            return (Angle(targetDir, targetForward),
                targetDir.y);
        }

        internal static void AngleClamp(this (float, float) _this)
        {
            _this.Item1.AngleClamp();
            _this.Item2.AngleClamp();
        }

        internal static void AngleClamp(this float _this)
        {
            _this = 360.0f - _this;
            while (_this > 359.9999f)
                _this -= 360.0f;
        }

        private static float Angle(Vector3 fwd, Vector3 targetDir)
        {
            var fwd2d = new Vector3(fwd.x, 0, fwd.z);
            var targetDir2d = new Vector3(targetDir.x, 0, targetDir.z);
            var angle = Vector3.Angle(fwd2d, targetDir2d);
            if (AngleDir(fwd, targetDir, Vector3.up) == -1)
            {
                angle.AngleClamp();
                return angle;
            }
            return angle;
        }

        internal static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            var perp = Vector3.Cross(fwd, targetDir);
            var dir = Vector3.Dot(perp, up);
            if (dir > 0.0)
                return 1;
            if (dir < 0.0)
                return -1;
            return 0;
        }
    }
}