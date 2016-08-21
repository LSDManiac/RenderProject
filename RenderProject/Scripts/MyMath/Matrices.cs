using System;

namespace RenderProject.MyMath
{
    public struct Matrix
    {
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            _values = new double[rows * columns];
        }

        public static Matrix IdentityMatrix(int size)
        {
            Matrix newM  = new Matrix(size, size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newM[i, j] = i == j ? 1 : 0;
                }
            }

            return newM;
        }

        public int rows { get; }
        public int columns { get; }
        private readonly double[] _values;

        public double this[int i, int j]
        {
            get { return _values[i * columns + j]; }
            set { _values[i * columns + j] = value; }
        }

        #region Opearors

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.columns != m2.rows)
            {
                throw new ArgumentException("Matrix m1.columns != matrix m2.rows");
            }

            Matrix newM = new Matrix(m1.rows, m2.columns);

            int m = m1.columns;

            for (int i = 0; i < newM.rows; i++)
            {
                for (int j = 0; j < newM.columns; j++)
                {
                    double result = 0;
                    for (int k = 0; k < m; k++)
                    {
                        result += m1[i, k] * m2[k, j];
                    }
                    newM[i, j] = result;
                }
            }

            return newM;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.columns != m2.columns)
            {
                throw new ArgumentException("Matrix m1.columns != matrix m2.columns");
            }
            if (m1.rows != m2.rows)
            {
                throw new ArgumentException("Matrix m1.rows != matrix m2.rows");
            }
            
            Matrix newM = new Matrix(m1.rows, m1.columns);
            
            for (int i = 0; i < newM.rows; i++)
            {
                for (int j = 0; j < newM.columns; j++)
                {
                    newM[i, j] = m1[i, j] + m2[i, j];
                }
            }

            return newM;
        }

        public static Matrix operator *(Matrix m, double k)
        {
            Matrix newM = new Matrix(m.rows, m.columns);

            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.columns; j++)
                {
                    newM[i, j] = m[i, j] * k;
                }
            }

            return newM;
        }

        public static Matrix operator *(double k, Matrix m)
        {
            return m * k;
        }

        public Matrix transpose
        {
            get
            {
                Matrix newM = new Matrix(columns, rows);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        newM[j, i] = this[i, j];
                    }
                }

                return newM;
            }
        }

        #endregion



    }
}
