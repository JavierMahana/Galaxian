using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace Galaxian
{
    class Galaxian
    {
        public int Width { get; private set; } 
        public int Height { get; private set; }

        public Player player = null;
        public Bullet pBullet = null;
        //public System.Windows.Point PBulletSpawnPoint => 

        private bool pBulletActive = false;

        private Random rand;

        public List<Enemy> enemies = new List<Enemy>(40);
        private int originXOffset;
        private System.Windows.Point enemiesOrigin;
        private float originSpeed;
        private int inbetweenSpace;
        private int enemySize;
        private int originSpots;
        private int currOriginSpot;
        private bool originDirection;

        public void MoveOrigin(long deltaTime)
        {
            int desiredOriginPosX = originXOffset + currOriginSpot * (enemySize + inbetweenSpace);
            if (enemiesOrigin.X != desiredOriginPosX)
            {
                int newOriginPosX;
                //se mueve a la derecha
                if (originDirection)
                {
                    newOriginPosX = (int)(enemiesOrigin.X + originSpeed * deltaTime);
                    if (newOriginPosX >= desiredOriginPosX)
                    {
                        newOriginPosX = desiredOriginPosX;
                    }
                }
                //se mueve a la izquierda
                else
                {
                    newOriginPosX = (int)(enemiesOrigin.X + -originSpeed * deltaTime);
                    if (newOriginPosX <= desiredOriginPosX)
                    {
                        newOriginPosX = desiredOriginPosX;
                    }
                }
                enemiesOrigin = new System.Windows.Point(newOriginPosX, enemiesOrigin.Y);
                //int newOriginPos = enemiesOrigin 
            }
            else 
            {
                int nextSpot = rand.Next(originSpots);
                if (currOriginSpot < nextSpot)
                {
                    originDirection = true;
                }
                else 
                {
                    originDirection = false;
                }
                currOriginSpot = nextSpot;
            }
        }

        public System.Windows.Point CalculateEnemyGridPosition(int xCoord, int yCoord)
        {
            float xOffset = xCoord * (enemySize + inbetweenSpace);
            float yOffset = yCoord * (enemySize + inbetweenSpace);

            return new System.Windows.Point(enemiesOrigin.X + xOffset, enemiesOrigin.Y + yOffset);
        }


        public System.Windows.Point GetBulletSpawnPoint()
        {
            if (player != null && pBullet != null)
            {
                return new System.Windows.Point(player.Position.X + player.Width / 2 - pBullet.Width, player.Position.Y - pBullet.Height);
            }
            else 
            {
                Console.WriteLine($"El player es null {player == null} | La bala es null {pBullet == null}");
                return new System.Windows.Point(0, 0);
            }
            
        }

        public bool OutOfBounds(Rect boundingBox)
        {
            Rect windowBounds = new Rect(0, 0, Width, Height);
            return  !windowBounds.IntersectsWith(boundingBox);
        }

        public void MovePlayer(long deltaTime, bool toTheRight)
        {
            if (toTheRight)
            {
                player.Move(new Vector(player.speed * deltaTime, 0));
                Console.WriteLine($"Moving the player {player.speed * deltaTime}");
            }
            else 
            {
                player.Move(new Vector(-player.speed * deltaTime, 0));
                Console.WriteLine($"Moving the player {-player.speed * deltaTime}");
            }
        }

        public void MoveAllEnemies()
        {
            foreach (var enemy in enemies)
            {
                MoveEnemy(enemy);
            }
        }

        public void MoveEnemy(Enemy enemy)
        {
            if (enemy.isActive)
            {
            }
            else 
            {
                enemy.MoveTo(CalculateEnemyGridPosition(enemy.coordX, enemy.coordY));
            }
        }
        public void PBulletRestart()
        {
            pBullet.MoveTo(GetBulletSpawnPoint());
            pBulletActive = false;
        }

        public void Paint(Graphics dc)
        {
            //Console.WriteLine($"rect player: {player.BoundingBox}| rect pBullet: {pBullet.BoundingBox}");
            //Console.WriteLine($"size player: X{(int)player.Width}| Y{(int)player.Height}");

            Brush whiteBrush = new SolidBrush(Color.White);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Brush redBrush = new SolidBrush(Color.Red);

            dc.FillRectangle(whiteBrush, new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Width, (int)player.Height));
            dc.FillRectangle(yellowBrush, new Rectangle((int)pBullet.Position.X, (int)pBullet.Position.Y, (int)pBullet.Width, (int)pBullet.Height));
            foreach (var enemy in enemies)
            {
                dc.FillRectangle(redBrush, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));
            }
        }

        public void PBulletCollisionCheck()
        {
            foreach (var enemy in enemies)
            {
                if (Collisionable.Collide(enemy, pBullet))
                {
                    PBulletRestart();
                    enemies.Remove(enemy);
                    return;
                }
            }
        }
        public void UpdatePBullet(long deltaTime)
        {
            if (pBulletActive)
            {
                pBullet.Update(deltaTime);
                PBulletCollisionCheck();
                
                if (OutOfBounds(pBullet.BoundingBox))
                {
                    PBulletRestart();
                }
            }
            else 
            {
                //se mueve arriba del jugador.
                pBullet.MoveTo(GetBulletSpawnPoint());
            }
        }
        public void PlayerShoot()
        {
            if (!pBulletActive)
            {
                pBulletActive = true;
            }
        }
        public Galaxian(int width, int height, System.Windows.Point playerSize, float playerSpd, int enemySize, int enemyOffset, System.Windows.Point originPos)
        {
            rand = new Random();
            Width = width;
            Height = height;

            enemiesOrigin = originPos;
            this.enemySize = enemySize;
            inbetweenSpace = enemyOffset;
            originXOffset = (int)originPos.X;
            originSpeed = 0.2f;
            originSpots = 7;
            currOriginSpot = 6;
            originDirection = true;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var pos = CalculateEnemyGridPosition(x, y);
                    enemies.Add( new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y));
                }
            }

            //valores 
            player = new Player(new Rect(width / 2, height - (playerSize.Y + 30), playerSize.X, playerSize.Y), playerSpd);
            pBullet = new Bullet(new Rect(GetBulletSpawnPoint().X, GetBulletSpawnPoint().Y, 4, 6), new Vector(0, -0.6f));

            

            pBulletActive = false;
        }
    }
}
