using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEngine.managers
{
    public static class MouseManager
    {
        private static Point Location;
        private static int ButtonPressed = -1;

        public static void UpdateLocation(int x, int y)
        {
            Location.X = x;
            Location.Y = y;
        }

        public static Point Get()
        {
            return new Point(Location.X, Location.Y);
        }

        public static void UpdateMouseButton(int button)
        {
            ButtonPressed = button;
        }

        public static int MouseButton()
        {
            return ButtonPressed;
        }
    }
}
