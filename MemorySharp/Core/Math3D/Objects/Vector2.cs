using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Binarysharp.MemoryManagement.Core.Math3D.Objects
{
    /// <summary>
    ///     Class that holds information about a 2d-coordinate and offers some basic operations.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        ///     The x axis.
        /// </summary>
        public float X;

        /// <summary>
        ///     The y axis.
        /// </summary>
        public float Y;

        /// <summary>
        ///     Initializes a new Vector2 using the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Initializes a new Vector2 by copying the values of the given Vector2
        /// </summary>
        /// <param name="vec"></param>
        public Vector2(Vector2 vec) : this(vec.X, vec.Y)
        {
        }

        /// <summary>
        ///     Initializes a new Vector2 using the given float-array
        /// </summary>
        /// <param name="values"></param>
        public Vector2(IReadOnlyList<float> values) : this(values[0], values[1])
        {
        }

        /// <summary>
        ///     Returns a new Vector3 at (1,0)
        /// </summary>
        public static Vector2 UnitX => new Vector2(1, 0);

        /// <summary>
        ///     Returns a new Vector2 at (0,1)
        /// </summary>
        public static Vector2 UnitY => new Vector2(0, 1);

        /// <summary>
        ///     Returns a new Vector2 at (0,0)
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);

        /// <summary>
        ///     Indexor for the vector <code>float x,y,z</code>
        /// </summary>
        /// <param name="i"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
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

                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Determines the distance to another <code>Vector2</code>.
        /// </summary>
        /// <param name="other">The other <code>Vector2</code> to calculate the distance from.</param>
        /// <returns>A <code>float</code> distance between the two vectors.</returns>
        public float DistanceTo(Vector2 other)
        {
            return (this + other).Length();
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
            var vec = (Vector2) obj;
            return GetHashCode() == vec.GetHashCode();
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        /// <summary>
        ///     Returns the length of this Vector2
        /// </summary>
        /// <returns></returns>
        public float Length() => (float) Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        /// <summary>
        ///     To the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string ToString()
            => $"[X={X.ToString(CultureInfo.InvariantCulture)}, Y={Y.ToString(CultureInfo.InvariantCulture)}]";

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.X - v2.X, v1.Y - v2.Y);

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);

        /// <summary>
        ///     Implements the operator *.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector2 operator *(Vector2 v1, float scalar) => new Vector2(v1.X*scalar, v1.Y*scalar);

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.X + v2.X, v1.Y + v2.Y);

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public static bool operator ==(Vector2 v1, Vector2 v2) => v1.X == v2.X && v1.Y == v2.Y;
    }
}