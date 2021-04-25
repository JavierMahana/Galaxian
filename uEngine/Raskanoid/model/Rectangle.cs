using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaskanoidGame.model
{
    public class Rectangle
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public System.Drawing.Rectangle Parse()
        {
            return new System.Drawing.Rectangle(X, Y, Width, Height);
        }

        public bool Intersects(Rectangle rectangle)
        {
            int ax1 = X;
            int ax2 = X + Width;
            int ay1 = Y;
            int ay2 = Y + Height;

            int bx1 = rectangle.X;
            int bx2 = rectangle.X + rectangle.Width;
            int by1 = rectangle.Y;
            int by2 = rectangle.Y + rectangle.Height;

            return ((ax1 <= bx1 && bx1 <= ax2) || (ax1 <= bx2 && bx2 <= ax2)) && 
                ((ay1 <= by1 && by1 <= ay2) || (ay1 <= by2 && by2 <= ay2));

        }
    }
}
