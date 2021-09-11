using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Drawer
    {
        static float x, y;
        static Graphics graphics;
        public static void Initialize(Graphics newGraphics)
        {
            graphics = newGraphics;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        { x = x0; y = y0; }
        public static void MakeIt(Pen pen, double length, double angle)
        {
            //Делает шаг длиной dlina в направлении ugol и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        private const double Pi = Math.PI;
        static readonly double SquareRootOf2 = Math.Sqrt(2);
        public static void Draw(int width, int height, double rotationAngle, Graphics graphics)
        {
            Drawer.Initialize(graphics);

            var sz = Math.Min(width, height);

            var diagonal_length = Math.Sqrt(2) * (sz * 0.375f + sz * 0.04f) / 2;
            var x0 = (float)(diagonal_length * Math.Cos(Pi / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonal_length * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            Drawer.SetPosition(x0, y0);

            //Рисуем 1-ую сторону
            DrawerMakeHelper(sz, 0, Pi / 4, Pi, Pi/2);
            DrawerChangeHelper(sz, -Pi, 3 * Pi / 4);

            //Рисуем 2-ую сторону
            DrawerMakeHelper(sz, -Pi / 2, -Pi / 2 + Pi / 4, -Pi / 2 + Pi, -Pi/2 + Pi/2);
            DrawerChangeHelper(sz, -Pi / 2 -Pi, -Pi/2 + 3*Pi/4);

            //Рисуем 3-ю сторону
            DrawerMakeHelper(sz, Pi, Pi + Pi / 4, 2 * Pi, Pi + Pi / 2);
            DrawerChangeHelper(sz, Pi - Pi, Pi + 3 * Pi / 4);

            //Рисуем 4-ую сторону
            DrawerMakeHelper(sz, Pi / 2, Pi / 2 + Pi / 4, Pi / 2 + Pi, Pi);
            DrawerChangeHelper(sz, Pi / 2 - Pi, Pi / 2 + 3 * Pi / 4);
        }

        public static void DrawerMakeHelper(double sz, double first, double second, double third, double fourth)
        {
            Drawer.MakeIt(Pens.Yellow, sz * 0.375f, first);
            Drawer.MakeIt(Pens.Yellow, sz * 0.04f * SquareRootOf2, second);
            Drawer.MakeIt(Pens.Yellow, sz * 0.375f, third);
            Drawer.MakeIt(Pens.Yellow, sz * 0.375f - sz * 0.04f, fourth);
        }

        public static void DrawerChangeHelper(double sz, double first, double second)
        {
            Drawer.Change(sz * 0.04f, first);
            Drawer.Change(sz * 0.04f * SquareRootOf2, second);
        }
    }
}