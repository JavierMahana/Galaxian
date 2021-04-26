using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Galaxian
{
    class Player : Collisionable
    {
        public float speed;
        public Player(Rect boundingBox, float speed) : base(boundingBox)
        {
            this.speed = speed;
        }
    }
}
