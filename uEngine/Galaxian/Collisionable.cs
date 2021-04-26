using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Galaxian
{
    abstract class Collisionable
    {
        public Point Position => BoundingBox.Location;
        public double Width => BoundingBox.Width;
        public double Height => BoundingBox.Height;


        public Rect BoundingBox { get; private set; }
        public Collisionable(Rect boundingBox)
        {
            BoundingBox = boundingBox;
        }
 
        public void MoveTo(Point point)
        {
            Vector movement = point - BoundingBox.Location;
            Move(movement);
        }
        public void Move(Vector ammount)
        {
            Console.WriteLine($"moviendo: {ammount}");
            BoundingBox = new Rect(BoundingBox.X + ammount.X, BoundingBox.Y + ammount.Y, BoundingBox.Width, BoundingBox.Height);
            //BoundingBox.Offset(ammount);
        }
        public static bool Collide(Collisionable a, Collisionable b)
        {
            return a.BoundingBox.IntersectsWith(b.BoundingBox);
        }

    }
}
