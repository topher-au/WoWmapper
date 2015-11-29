using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Binarysharp.MemoryManagement.Core.Math3D.Objects
{
    /// <summary>
    ///     Class that holds information about a 3d-coordinate and offers some basic operations.
    /// </summary>
    public struct Vector3
    {
        /// <summary>
        ///     The X axis.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        ///     The Y axis.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        ///     The Z axis of the vector3.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        ///     Initializes a new Vector3 using the given values.
        /// </summary>
        /// <param name="x">The X location of the vector3.</param>
        /// <param name="y">The Y location of the vector3.</param>
        /// <param name="z">The Z location of the vector3.</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        ///     Initializes a new Vector3 by copying the values of the given Vector3.
        /// </summary>
        /// <param name="vec">The vector3 to copy.</param>
        public Vector3(Vector3 vec) : this(vec.X, vec.Y, vec.Z)
        {
        }

        /// <summary>
        ///     Initializes a new Vector3 using the given float-array.
        /// </summary>
        /// <param name="values">An array of floats containing [0] X float [1] Y float [2] Z float.</param>
        public Vector3(IReadOnlyList<float> values) : this(values[0], values[1], values[2])
        {
        }

        /// <summary>
        ///     Returns a new Vector3 at (1,0,0).
        /// </summary>
        public static Vector3 UnitX => new Vector3(1, 0, 0);

        /// <summary>
        ///     Returns a new Vector3 at (0,1,0).
        /// </summary>
        public static Vector3 UnitY => new Vector3(0, 1, 0);

        /// <summary>
        ///     Returns a new Vector3 at (0,0,1).
        /// </summary>
        public static Vector3 UnitZ => new Vector3(0, 0, 1);

        /// <summary>
        ///     Returns a new Vector3 at (0,0,0).
        /// </summary>
        public static Vector3 Zero => new Vector3(0, 0, 0);

        /// <summary>
        ///     Indexer.
        /// </summary>
        /// <param name="i">Index of the X-Y-Z float array.</param>
        /// <exception cref="IndexOutOfRangeException">The index accsessed was > [0]/[1]/[3].</exception>
        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return X;

                    case 1:
                        return Y;

                    case 2:
                        return Z;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        X = value;
                        break;

                    case 1:
                        Y = value;
                        break;

                    case 2:
                        Z = value;
                        break;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Returns the distance from this Vector3 to the given Vector3.
        /// </summary>
        /// <param name="other">Another <see cref="Vector3" /> struct to calculate distance to.</param>
        /// <returns>The dinstance between this vector3 and the given vector3.</returns>
        public float DistanceTo(Vector3 other)
        {
            return (this - other).Length();
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var vec = (Vector3)obj;
            return GetHashCode() == vec.GetHashCode();
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

        /// <summary>
        ///     Calculates the length of this Vector3.
        /// </summary>
        /// <returns>The length of this Vecto3r</returns>
        public float Length()
            =>
                (float)
                    Math.Abs(
                        Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"[X={X}, Y={Y}, Z={Z}]";
        }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        ///     Implements the operator *.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator *(Vector3 v1, float scalar)
        {
            return new Vector3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
    }
}