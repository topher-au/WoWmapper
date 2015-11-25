using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Binarysharp.MemoryManagement.Core.Math3D.Objects
{
    /// <summary>
    ///     Class that holds information about a 3d-coordinate and offers some basic operations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Location
    {
        /// <summary>
        ///     The x axis.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        ///     The y axis.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        ///     The z axis.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Location" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Location(float x, float y, float z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Location" /> struct.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public Location(XElement xml)
            : this(Convert.ToSingle(xml.Attribute("X").Value),
                Convert.ToSingle(xml.Attribute("Y").Value),
                Convert.ToSingle(xml.Attribute("Z").Value))
        {
        }

        /// <summary>
        ///     The length.
        /// </summary>
        public double Length => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        /// <summary>
        ///     Gets the <code>float</code> angle of the vector.
        /// </summary>
        public float Angle => (float) Math.Atan2(Y, X);

        /// <summary>
        ///     Gets a new location instance with 0,0,0 as te values.
        /// </summary>
        public static Location Zero => new Location(0, 0, 0);

        /// <summary>
        ///     Distances to.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns>System.Double.</returns>
        public double DistanceTo(Location loc)
        {
            return
                Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2) +
                          Math.Pow(Z - loc.Z, 2));
        }

        /// <summary>
        ///     Determines the distance to another <code>Location</code>.
        /// </summary>
        /// <param name="other">The other <code>Location</code> to calculate the distance from.
        public double Distance2D(Location other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        /// <summary>
        ///     Normalizes this instance.
        /// </summary>
        /// <returns>Binarysharp.MemoryManagement.Math.Location.</returns>
        public Location Normalize()
        {
            var len = Length;
            return new Location((float) (X/len), (float) (Y/len), (float) (Z/len));
        }


        /// <summary>
        ///     Returns a <code>Vector3(x,y,z)</code> with using the <code>X,Y,Z</code> data from this instance.
        /// </summary>
        /// <returns>Binarysharp.MemoryManagement.Math.Vector3.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Boolean.</returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var loc = (Location) obj;
            return loc.X == X && loc.Y == Y && loc.Z == Z;
        }


        /// <summary>
        ///     Serves as the hash function for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode();
        }


        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "[" + (int) X + ", " + (int) Y + ", " + (int) Z + "]";
        }

        /// <summary>
        ///     Gets the <code>Location</code> data as a <code>XML</code> element with the <code>X,Y,Z</code> as the three
        ///     attributes of the element.
        /// </summary>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement GetXml()
        {
            return new XElement("Location", new XAttribute("X", X), new XAttribute("Y", Y), new XAttribute("Z", Z));
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Location a, Location b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Location a, Location b)
        {
            return !a.Equals(b);
        }
    }
}