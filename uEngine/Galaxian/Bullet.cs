using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Galaxian
{
    class Bullet : Collisionable
    {
        private Vector velocity;
        public bool Activo = false;


        public Bullet(Rect boundingBox, Vector vel) : base(boundingBox)
        {
            velocity = vel ;
        }
        //public void MoveTo
        public void Update(long deltaTime)
        {
             Move(velocity * deltaTime);
        }

    }
}
