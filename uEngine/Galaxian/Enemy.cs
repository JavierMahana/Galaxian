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

        public Vector currVelocity;

        public Vector startingVelocity;
        public float maxSpeed;
        public float maxAceleration;

        

        public void Launch()
        {
            currVelocity = startingVelocity;
            isActive = true;
        }
        public void Reset()
        {
            isActive = false;
        }

        public void UpdateOnAttack(Point targetPosition, long deltaTime)
        {
            Vector desiredDir = targetPosition - Position;
            desiredDir.Normalize();
            //desiredVel *= maxSpeed;

            Vector apliedForce = desiredDir * maxAceleration;
            currVelocity += apliedForce;
            if (currVelocity.Length >= maxSpeed)
            {
                currVelocity.Normalize();
                currVelocity *= maxSpeed;
            }

            Move(currVelocity * deltaTime);
        }
        public Enemy(Rect bBox, int coordX, int coordY, Vector startingVelocity, float maxSpeed, float maxAceleration) : base(bBox)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            isActive = false;
            isParking = false;

            this.startingVelocity = startingVelocity;
            this.maxSpeed = maxSpeed;
            this.maxAceleration = maxAceleration;
        }
    }
}
