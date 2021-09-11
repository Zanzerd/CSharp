using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var sWidth = sx.GetLength(0);
            var sHeight = sx.GetLength(1);
            double[,] sy = Transpose(sx);
            var result = new double[width, height];
            int borderX = (int)Math.Floor((double)sWidth/2);
            int borderY = (int)Math.Floor((double)sHeight/2);
            for (int x = borderX; x < width - borderX; x++)
                for (int y = borderY; y < height - borderY; y++)
                {
                    double[,] window = new double[sWidth, sHeight];
                    for (int i = 0; i < sWidth; i++) 
                        for (int j = 0; j < sHeight; j++)
                        {
                            window[i, j] = g[x - borderX + i, y - borderY + j];
                        }
                    double[,] gxTemp = MatrixMatrixMultiply(sx, window);
                    double gx = SumOfMatrix(gxTemp);
                    double[,] gyTemp = MatrixMatrixMultiply(sy, window);
                    double gy = SumOfMatrix(gyTemp);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }

        public static double[,] MatrixMatrixMultiply(double[,] A, double[,] B)
        {
            int Ax = A.GetLength(0);
            int Ay = A.GetLength(1);
            int Bx = B.GetLength(0);
            int By = B.GetLength(1);
            if (Ax != Bx || Ay != By) 
                throw new Exception("Матрицы нельзя перемножить, размерности не совпадают\n");
            double[,] result = new double[Ax,By];
            for (int i = 0; i < Ax; i++)
            {
                for (int j = 0; j < Ay; j++)
                {
                    result[i, j] = A[i, j] * B[i, j];
                }
            }
            return result;
        }

        public static double[,] Transpose(double[,] A)
        {
            int Ax = A.GetLength(0);
            int Ay = A.GetLength(1);
            double[,] AT = new double[Ax, Ay];
            for (int i = 0; i < Ax; i++)
            {
                double[] row = new double[Ay];
                for (int j = 0; j < Ay; j++)
                {
                    row[j] = A[i, j];
                }

                for (int k = 0; k < Ay; k++)
                {
                    AT[k, i] = row[k];
                }
            }
            return AT;
        }

        public static double SumOfMatrix(double[,] A)
        {
            double sum = 0;
            int Ax = A.GetLength(0);
            int Ay = A.GetLength(1);
            for (int i = 0; i < Ax; i++)
                for (int j = 0; j < Ay; j++)
                    sum += A[i, j];
            return sum;
        }
    }
}