using System;
using Binarysharp.MemoryManagement.Core.Helpers;

namespace Binarysharp.MemoryManagement.Core.Math3D.Objects
{
    /// <summary>
    ///     Encapsulates a 3-by-3 affine matrix that represents a geometric transform.
    /// </summary>
    public struct Matrix
    {
        private int Columns { get; }
        private float[] Data { get; }
        private int Rows { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="rows">Number of rows in matrix.</param>
        /// <param name="columns">Number of columns in matrix.</param>
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Data = new float[rows*columns];
        }

        /// <summary>
        ///     Gets or sets the <see cref="float" /> with the specified index of the matrix.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Single.</returns>
        public float this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        ///     Gets or sets the <see cref="float" /> with the specified row and column.
        /// </summary>
        /// <param name="row">The matrix row.</param>
        /// <param name="column">The matrix column.</param>
        /// <returns>System.Single.</returns>
        public float this[int row, int column]
        {
            get { return Data[row*Columns + column]; }
            set { Data[row*Columns + column] = value; }
        }

        /// <summary>
        ///     Reads the specified data from an array of bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Read(byte[] data)
        {
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Columns; x++)
                {
                    this[y, x] = BitConverter.ToSingle(data, TypeData<float>.Size)*((y*Columns) + x);
                }
            }
        }

        /// <summary>
        ///     Converts the matrix structure to a byte array.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] ToByteArray()
        {
            var data = new byte[Data.Length*TypeData<float>.Size];
            for (var i = 0; i < Data.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(Data[i]), 0, data, i*TypeData<float>.Size, TypeData<float>.Size);
            }
            return data;
        }
    }
}