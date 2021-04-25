using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System

namespace Galaxian
{


    class Enemy : Collisionable
    {
        public int coordX;
        public int coordY;
        public bool isActive;
        public bool isParking;
        //launchAngle
        public Enemy(Rect bBox, int coordX, int coordY) : base(bBox)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            isActive = false;
            isParking = false;
        }
    }
}
