using Binarysharp.MemoryManagement.Core.Math3D.Objects;
using System;

//using Binarysharp.MemoryManagement.Math;

namespace Binarysharp.MemoryManagement.Core.Math3D
{
    /// <summary>
    ///     A utility-class that offers several mathematical algorithms.
    ///     <remarks> Credits for ths class go to Zat@unknowncheats</remarks>
    ///     .
    /// </summary>
    public static class DimensionaMathExtensions
    {
        #region Fields, Private Properties

        private const float Deg2Rad = (float)(Math.PI / 180f);
        private const float Rad2Deg = (float)(180f / Math.PI);

        #endregion Fields, Private Properties

        /// <summary>
        ///     Calculates an angle that aims from the given source-Vector3 to the given destination-Vector3
        /// </summary>
        /// <param name="src">3d-coordinate of where to aim from.</param>
        /// <param name="dst">3d-coordinate of where to aim to.</param>
        /// <returns></returns>
        public static Vector3 CalculateAngle(this Vector3 src, Vector3 dst)
        {
            var ret = new Vector3();
            var vDelta = src - dst;
            var fHyp = (float)Math.Sqrt((vDelta.X * vDelta.X) + (vDelta.Y * vDelta.Y));

            ret.X = RadiansToDegrees((float)Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }

        /// <summary>
        ///     Clamps a given angle.
        /// </summary>
        /// <param name="qaAng">Angle to clamp.</param>
        /// <returns>Clamped angle.</returns>
        public static Vector3 ClampAngle(this Vector3 qaAng)
        {
            //if (qaAng.X > 89.0f && qaAng.X <= 180.0f)
            //    qaAng.X = 89.0f;

            //while (qaAng.X > 180.0f)
            //    qaAng.X = qaAng.X - 360.0f;

            //if (qaAng.X < -89.0f)
            //    qaAng.X = -89.0f;

            //while (qaAng.Y > 180.0f)
            //    qaAng.Y = qaAng.Y - 360.0f;

            //while (qaAng.Y < -180.0f)
            //    qaAng.Y = qaAng.Y + 360.0f;

            //return qaAng;
            if (qaAng[0] > 89.0f)
                qaAng[0] = 89.0f;

            if (qaAng[0] < -89.0f)
                qaAng[0] = -89.0f;

            while (qaAng[1] > 180)
                qaAng[1] -= 360;

            while (qaAng[1] < -180)
                qaAng[1] += 360;

            qaAng.Z = 0;

            return qaAng;
        }

        /// <summary>
        ///     Copies an array of vectors to a new array containing identical, new Vector3s (deep-copy)
        /// </summary>
        /// <param name="source">Source-array to copy from</param>
        /// <returns>New array containing identical yet new Vector3s.</returns>
        public static Vector3[] CopyVectors(this Vector3[] source)
        {
            var ret = new Vector3[source.Length];
            for (var i = 0; i < ret.Length; i++)
                ret[i] = new Vector3(source[i]);
            return ret;
        }

        /// <summary>
        ///     Converts the given angle in degrees to radians.
        /// </summary>
        /// <param name="deg">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static float DegreesToRadians(float deg) => deg * Deg2Rad;

        /// <summary>
        ///     Applies (adds) an offset to an array of 3d-coordinates.
        /// </summary>
        /// <param name="offset">Offset to apply.</param>
        /// <param name="points">Array if 3d-coordinates.</param>
        /// <returns>Array of manipulated 3d-coordinates.</returns>
        public static Vector3[] OffsetVectors(this Vector3 offset, params Vector3[] points)
        {
            for (var i = 0; i < points.Length; i++)
            {
                points[i] += offset;
            }
            return points;
        }

        /// <summary>
        ///     Returns whether the given point is within a circle of the given radius around the given center
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <param name="circleCenter">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>
        ///     If the given point is within a circle of the given radius around the given center, <code>true</code>, else
        ///     <code>false</code>.
        /// </returns>
        public static bool PointInCircle(this Vector2 point, Vector2 circleCenter, float radius)
        {
            return (point - circleCenter).Length() < radius;
        }

        /// <summary>
        ///     Converts the given angle in radians to degrees.
        /// </summary>
        /// <param name="rad">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static float RadiansToDegrees(float rad)
        {
            return rad * Rad2Deg;
        }

        /// <summary>
        ///     Rotates a given point around another point.
        /// </summary>
        /// <param name="pointToRotate">Point to rotate.</param>
        /// <param name="centerPoint">Point to rotate around.</param>
        /// <param name="angleInDegrees">Angle of rotation in degrees.</param>
        /// <returns>Rotated point as a <code>Vector2</code></returns>
        public static Vector2 RotatePoint(this Vector2 pointToRotate, Vector2 centerPoint, float angleInDegrees)
        {
            var angleInRadians = (float)(angleInDegrees * (Math.PI / 180f));
            var cosTheta = (float)Math.Cos(angleInRadians);
            var sinTheta = (float)Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                           (int)
                           (cosTheta * (pointToRotate.X - centerPoint.X) -
                            sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                           (int)
                           (sinTheta * (pointToRotate.X - centerPoint.X) +
                            cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        /// <summary>
        ///     Smooths an angle from source to destinations.
        /// </summary>
        /// <param name="src">Original angle.</param>
        /// <param name="dest">Destination angle.</param>
        /// <param name="smoothAmount">Value between 0 and 1 to apply as smooting where 0 is no modification and 1 is no smoothing.</param>
        /// <returns>A new <code>Vector3</code> with the smoothed angle.</returns>
        public static Vector3 SmoothAngle(this Vector3 src, Vector3 dest, float smoothAmount)
        {
            return src + (dest - src) * smoothAmount;
        }

        /// <summary>
        ///     Translates an array of 3d-coordinates to screen-coodinates.
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation.</param>
        /// <param name="screenSize">The size of the screen which is translated to.</param>
        /// <param name="points">Array of 3d-coordinates.</param>
        /// <returns>Array of translated screen-coodinates.</returns>
        public static Vector2[] WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, params Vector3[] points)
        {
            var worlds = new Vector2[points.Length];
            for (var i = 0; i < worlds.Length; i++)
            {
                worlds[i] = viewMatrix.WorldToScreen(screenSize, points[i]);
            }
            return worlds;
        }

        /// <summary>
        ///     Translates a 3d-coordinate to a screen-coodinate
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation.</param>
        /// <param name="screenSize">The size of the screen which is translated to.</param>
        /// <param name="point3D">3d-coordinate of the point to translate.</param>
        /// <returns>Translated screen-coodinate.</returns>
        public static Vector2 WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, Vector3 point3D)
        {
            var returnVector = Vector2.Zero;
            var w = viewMatrix[3, 0] * point3D.X + viewMatrix[3, 1] * point3D.Y + viewMatrix[3, 2] * point3D.Z +
                    viewMatrix[3, 3];
            if (!(w >= 0.01f)) return returnVector;
            var inverseX = 1f / w;
            returnVector.X =
                (screenSize.X / 2f) +
                (0.5f * (
                    (viewMatrix[0, 0] * point3D.X + viewMatrix[0, 1] * point3D.Y + viewMatrix[0, 2] * point3D.Z +
                     viewMatrix[0, 3])
                    * inverseX)
                 * screenSize.X + 0.5f);
            returnVector.Y =
                (screenSize.Y / 2f) -
                (0.5f * (
                    (viewMatrix[1, 0] * point3D.X + viewMatrix[1, 1] * point3D.Y + viewMatrix[1, 2] * point3D.Z +
                     viewMatrix[1, 3])
                    * inverseX)
                 * screenSize.Y + 0.5f);
            return returnVector;
        }
    }
}