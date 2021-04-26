using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Drawing.Text;

namespace Galaxian
{
    class Galaxian
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public System.Drawing.Point PointLocation { get; private set; }

        public Player player = null;
        public Bullet pBullet = null;

        //public System.Windows.Point PBulletSpawnPoint => 

        private bool pBulletActive = false;
        private int puntaje;
        private int vidas;
        private string user;
        private string[] topUsers = { "", "", "" };
        private int[] topScore = { 0, 0, 0 };

        private Random rand;


        //public List<Enemy> enemies = new List<Enemy>(40);
        public List<Bullet> enemyBullets = new List<Bullet>(10);

        //public Bullet[] enemyBullets = new Bullet[10];
        public Enemy[,] enemies = new Enemy[10, 4];
        private int originXOffset;
        private System.Windows.Point bulletsOrigin;
        private System.Windows.Point enemiesOrigin;
        private float originSpeed;
        private int inbetweenSpace;
        private int enemySize;

        private int originSpots;
        private int currOriginSpot;
        private bool originDirection;

        private long lunchEnemyTimer;
        private long timeToLunchEnemy;

        private int eBulletWidth;
        private int eBulletHeight;
        private float eBulletSpeed;

        // menu
        private int contadorEstrellas;
        private int currMenu = 0;
        PrivateFontCollection pfc = new PrivateFontCollection();
        public SolidBrush BlueBrush = new SolidBrush(Color.Blue);
        public SolidBrush RedBrush = new SolidBrush(Color.Red);
        StringFormat drawFormat = new StringFormat();
        Font FontSmall = new Font("arial 12", 12);
        Font FontBig = new Font("arial 14", 12);

        Brush whiteBrush = new SolidBrush(Color.White);
        Brush yellowBrush = new SolidBrush(Color.Yellow);
        Brush redBrush = new SolidBrush(Color.Red);
        Brush pinkBrush = new SolidBrush(Color.LightPink);
        Brush lightBlueBrush = new SolidBrush(Color.LightBlue);
        Brush greenBrush = new SolidBrush(Color.Green);
        Brush lightGreenBrush = new SolidBrush(Color.LightGreen);
 
        public Rectangle[] estrellas = new Rectangle[16];
        private float estrellasSpeed;

        public Image playerImg;
        public Image enemy1Img;
        public Image enemy2Img;
        public Image enemy3Img;
        /*
        public Image nave1IMG;
        public Image nave2IMG;
        public Image nave3IMG;
        public Image playerIconIMG;
        public Image playerDeathIMG;
        public Image naveDeathIMG;*/

        ///   /   /   / Métodos
        // partida
        public bool VictoryCondition()
        {
            int scoreMinus;
            string nombreMinus;
            int contador = 0;
          
            if (vidas <= 0)
            {
                for (int i = 2; i > -1; i--)
                {
                    if (puntaje > topScore[i])
                    {
                        if (i == 2)
                        {
                            topScore[2] = puntaje;
                            topUsers[2] = user;
                        }
                        else
                        {
                            scoreMinus = topScore[i];
                            nombreMinus = topUsers[i];

                            topScore[i] = puntaje;
                            topUsers[i] = user;

                            topScore[i + 1] = scoreMinus;
                            topUsers[i + 1] = nombreMinus;

                        }
                    }
                }
                //revisar el archivo de texto y actualizar puntaje
                TextWriter writer = new StreamWriter("Puntaje.txt");
                for (int i = 0; i < 3; i++)
                {
                    writer.WriteLine(topUsers[i]);
                    writer.WriteLine(topScore[i]);
                }
                writer.Close();
                currMenu = 0;
                return false;
            }

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    contador++;
                }
            }

            if (contador == 0)
            {
                newScene(false);
                return true;
            }
            else
            {
                return true;
            }
        }
        public bool MenuOptions(System.Drawing.Point mousePosition)
        {
            int x = mousePosition.X;
            int y = mousePosition.Y;

            if (currMenu == 0)
            {
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 - 4 && y < Height / 2 + 20)
                {
                    currMenu = -1;
                    return true;
                }
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 + 60 - 4 && y < Height / 2 + 80)
                {
                    currMenu = 1;
                }
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 + 120 - 4 && y < Height / 2 + 140)
                {
                    currMenu = 2;
                }

            }
            if (currMenu == 1)
            {
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 + 120 - 4 && y < Height / 2 + 140)
                {
                    currMenu = 0;
                }
            }
            if (currMenu == 2)
            {
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 + 180 - 4 && y < Height / 2 + 200)
                {
                    currMenu = 0;
                }

            }
            if (currMenu == 3)
            {
                if (x > Width / 2 - 64 && x < Width / 2 + 80 && y > Height / 2 + 120 - 4 && y < Height / 2 + 140)
                {
                    currMenu = 0;
                }
            }

            return false;
        }
        public void newEnemies(Vector velInicial1, Vector velInicial2, float enemyMaxSpeed, float enemyMaxAccel)
        {

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    enemies[x, y] = null;
                }
            }

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var pos = CalculateEnemyGridPosition(x, y);
                    if (x <= 4)
                    {
                        if (y == 1)
                        {
                            enemies[x, y] = new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y, velInicial1, enemyMaxSpeed, enemyMaxAccel, rand.Next(2) + 1);
                        }
                        else
                        {
                            enemies[x, y] = new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y, velInicial1, enemyMaxSpeed, enemyMaxAccel, 1);
                        }
                    }
                    //la velocidad inicial es para la derecha
                    else
                    {
                        if (y == 1)
                        {
                            enemies[x, y] = new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y, velInicial2, enemyMaxSpeed, enemyMaxAccel, rand.Next(2) + 1);
                        }
                        else
                        {
                            enemies[x, y] = new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y, velInicial2, enemyMaxSpeed, enemyMaxAccel, 1);
                        }

                    }

                    if ((x == 3 || x == 5 || x == 7) && y == 0)
                    {
                        enemies[x, y] = new Enemy(new Rect(pos.X, pos.Y, enemySize, enemySize), x, y, velInicial1, enemyMaxSpeed, enemyMaxAccel, 3);
                    }
                }
            }
        }
        public void newScene(bool reinicioPuntaje)
        {
            if (reinicioPuntaje)
            {
                puntaje = 0;
                vidas = 3;
            }

            float enemyMaxSpeed = 0.5f;
            float enemyMaxAccel = 0.012f;

            float rad1 = (float)(225 * Math.PI) / 180;
            float velx1 = (float)(enemyMaxSpeed * Math.Cos(rad1));
            float vely1 = (float)(enemyMaxSpeed * Math.Sin(rad1));
            Vector velInicial1 = new Vector(velx1, vely1);

            float rad2 = (float)(315 * Math.PI) / 180;
            float velx2 = (float)(enemyMaxSpeed * Math.Cos(rad2));
            float vely2 = (float)(enemyMaxSpeed * Math.Sin(rad2));
            Vector velInicial2 = new Vector(velx2, vely2);

            newEnemies(velInicial1, velInicial2, enemyMaxSpeed, enemyMaxAccel);
        }


        // enemigos
        public System.Windows.Point CalculateEnemyGridPosition(int xCoord, int yCoord)
        {
            float xOffset = xCoord * (enemySize + inbetweenSpace);
            float yOffset = yCoord * (enemySize + inbetweenSpace);

            return new System.Windows.Point(enemiesOrigin.X + xOffset, enemiesOrigin.Y + yOffset);
        }

        public void MoveEnemyBullets(long deltaTime)
        {
            foreach (var bullet in enemyBullets)
            {

                bullet.Update(deltaTime);
                //if (bullet.Activo)
                //{
                    
                //}
            }
        }


        public void EnemyBulletCollisionCheck()
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (var bullet in enemyBullets)
            {
                if (Collisionable.Collide(bullet, player))
                {
                    bulletsToRemove.Add(bullet);
                    vidas--;
                }

                else if (OutOfBounds(bullet.BoundingBox))
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            foreach (var bull in bulletsToRemove)
            {
                enemyBullets.Remove(bull);
            }

        }
        public System.Windows.Point EnemyBulletSpawnPoint(Enemy enemy)
        {
            if (enemy != null)
            {
                return new System.Windows.Point(enemy.Position.X + enemy.Width / 2 - eBulletWidth, enemy.Position.Y - eBulletHeight);
            }
            else
            {
                //Console.WriteLine($"El player es null {player == null} | La bala es null {pBullet == null}");
                return new System.Windows.Point(0, 0);
            }

        }


        public void EnemyTryToShoot()
        {
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                if (enemy.Position.Y > Height / 2 && enemy.attack)
                {
                    enemy.attack = false;
                    var spawnPoint = EnemyBulletSpawnPoint(enemy);

                    var desiredVelocity = player.Position - enemy.Position;
                    desiredVelocity.Normalize();
                    desiredVelocity *= eBulletSpeed;

                    var newBullet = new Bullet(new Rect(spawnPoint.X, spawnPoint.Y, eBulletWidth, eBulletHeight), desiredVelocity);
                    enemyBullets.Add(newBullet);
                    //for( int i = 0; i < 10; i++)
                    //{
                    //    //chequea si esta aut of bounds

                    //    if (!enemyBullets[i].Activo)
                    //    {
                    //        enemyBullets[i].Activo = true;
                    //        enemyBullets[i].MoveTo(EnemyBulletSpawnPoint(enemy,enemyBullets[i]));
                    //        return;
                    //    }
                    //    //EnemyBulletCollisionCheck(deltaTime);
                    //}

                    
                    //return;
                }

            }

        }
        public void EnemycollisionCheck()
        {
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                if (Collisionable.Collide(enemy, player))
                {
                    vidas--;
                    enemies[enemy.coordX, enemy.coordY] = null;
                    return;
                }
            }
        }
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
        public void TryLunchEnemyAttack(long deltaTime)
        {
            lunchEnemyTimer += deltaTime;
            if (lunchEnemyTimer >= timeToLunchEnemy)
            {
                lunchEnemyTimer = 0;
                int v = rand.Next(2);

                //elige el de más a la izquierda
                if (v == 0)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            if (enemies[x, y] != null)
                            {
                                if (enemies[x, y].isActive == false && enemies[x, y].isParking == false)
                                {
                                    //enemies[x, y] = null;
                                    enemies[x, y].Launch();
                                    return;
                                    //enemies[x, y].Lunch();
                                }

                            }
                        }
                    }
                }
                //elige el de más a la derecha
                else
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 9; x >= 0; x--)
                        {
                            if (enemies[x, y] != null)
                            {
                                if (enemies[x, y].isActive == false && enemies[x, y].isParking == false)
                                {
                                    //enemies[x, y] = null;
                                    enemies[x, y].Launch();
                                    return;
                                    //enemies[x, y].Lunch();
                                }
                            }
                        }
                    }
                }
            }

        }
        public bool OutOfBounds(Rect boundingBox)
        {
            Rect windowBounds = new Rect(0, 0, Width, Height);
            return !windowBounds.IntersectsWith(boundingBox);
        }
        public void MoveAllEnemies(long deltaTime)
        {
            foreach (var enemy in enemies)
            {
                MoveEnemy(enemy, deltaTime);
            }
        }
        public void MoveEnemy(Enemy enemy, long deltaTime)
        {
            if (enemy == null)
            {
                return;
            }

            if (enemy.isActive)
            {
                enemy.UpdateOnAttack(player.Position, deltaTime);
                if (OutOfBounds(enemy.BoundingBox))
                {
                    enemy.Reset();
                }
            }
            else if (enemy.isParking)
            {
                var desiredPos = CalculateEnemyGridPosition(enemy.coordX, enemy.coordY);
                enemy.UpdateOnParking(desiredPos, deltaTime);
                //enemy.MoveTo();
            }
            else
            {
                enemy.MoveTo(CalculateEnemyGridPosition(enemy.coordX, enemy.coordY));
            }
        }


        // player and bullet
        public System.Windows.Point GetBulletSpawnPoint()
        {
            if (player != null && pBullet != null)
            {
                return new System.Windows.Point(player.Position.X + player.Width / 2 - pBullet.Width, player.Position.Y - pBullet.Height);
            }
            else
            {
                //Console.WriteLine($"El player es null {player == null} | La bala es null {pBullet == null}");
                return new System.Windows.Point(0, 0);
            }

        }
        public void PBulletRestart()
        {
            pBullet.MoveTo(GetBulletSpawnPoint());
            pBulletActive = false;
        }
        public void PBulletCollisionCheck()
            {
                foreach (var enemy in enemies)
                {

                    if (enemy == null)
                    {
                        continue;
                    }
                    if (Collisionable.Collide(enemy, pBullet))
                    {
                        PBulletRestart();
                        puntaje += enemy.points;
                        enemies[enemy.coordX, enemy.coordY] = null;
                        return;
                        //enemies.Remove(enemy);
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
        public void MovePlayer(long deltaTime, bool toTheRight)
        {
            if (toTheRight)
            {
                player.Move(new Vector(player.speed * deltaTime, 0));
                if (player.Position.X + player.Width >= Width)
                {
                    player.MoveTo(new System.Windows.Point(Width - player.Width, player.Position.Y));
                }
                //Console.WriteLine($"Moving the player {player.speed * deltaTime}");
            }
            else
            {
                player.Move(new Vector(-player.speed * deltaTime, 0));
                if (player.Position.X <= 0)
                {
                    player.MoveTo(new System.Windows.Point(0, player.Position.Y));
                }
                //Console.WriteLine($"Moving the player {-player.speed * deltaTime}");
            }
        }

        public void MoveEstrellas(long deltaTime)
        {
            for (int i = 0; i < 16; i++)
            {
                if (estrellas[i].Y > Height)
                {
                    estrellas[i].X = rand.Next(Width);
                    estrellas[i].Y = 0;
                }
                else 
                {
                    estrellas[i].Y += (int)(deltaTime * estrellasSpeed);
                }


            }
            //if (contadorEstrellas > 80)
            //{
            //    contadorEstrellas--;
            //}
            //else if (contadorEstrellas > 0 && contadorEstrellas <= 80)
            //{
            //    for (int i = 0; i < 16; i++)
            //    {
                    
            //    }
            //    contadorEstrellas--;
            //}
            //else
            //{

            //    contadorEstrellas = 100;
            //}

        }

        // pintar
        public void Paint(Graphics dc)
        {
            
            //Console.WriteLine($"rect player: {player.BoundingBox}| rect pBullet: {pBullet.BoundingBox}");
            //Console.WriteLine($"size player: X{(int)player.Width}| Y{(int)player.Height}");
            //pfc.AddFontFile("ARCADE_N.TTF");

          
            drawFormat.FormatFlags = StringFormatFlags.NoWrap;

            //pintar estrellas
            for (int i = 0; i < 16; i++)
            {
                if (i >= 0 && i <= 4)
                { dc.FillRectangle(pinkBrush, estrellas[i]); }
                if (i >= 4 && i <= 8)
                { dc.FillRectangle(lightBlueBrush, estrellas[i]); }
                if (i >= 8 && i <= 12)
                { dc.FillRectangle(whiteBrush, estrellas[i]); }
                if (i >= 12 && i <= 16)
                { dc.FillRectangle(lightGreenBrush, estrellas[i]); }
            }

            if (currMenu == 0)
            {
                dc.DrawString("Galaxian", FontBig, BlueBrush, Width / 2 - 80, Height / 2 - 100, drawFormat);
                dc.DrawString("Iniciar", FontSmall, RedBrush, Width / 2 - 64, Height / 2, drawFormat);
                dc.DrawString("Controles", FontSmall, RedBrush, Width / 2 - 64, Height / 2 + 60, drawFormat);
                dc.DrawString("Puntajes", FontSmall, RedBrush, Width / 2 - 64, Height / 2 + 120, drawFormat);
                dc.DrawString("---------------------------------", FontSmall, RedBrush, Width / 2 - 224, Height / 2 + 180, drawFormat);
                dc.DrawString("Desarrolladores", FontSmall, RedBrush, Width / 2 - 100, Height / 2 + 200, drawFormat);
                dc.DrawString("Javier Mahana", FontSmall, RedBrush, Width / 2 - 224, Height / 2 + 240, drawFormat);
                dc.DrawString("Joaquín Herrera", FontSmall, RedBrush, Width / 2 + 16, Height / 2 + 240, drawFormat);
                dc.DrawString("---------------------------------", FontSmall, RedBrush, Width / 2 - 224, Height / 2 + 260, drawFormat);
            }

            else if (currMenu == 1)
            {
                dc.DrawString("Controles", FontBig, BlueBrush, Width / 2 - 80, Height / 2 - 100, drawFormat);
                dc.DrawString("Movimiento, Teclas: [<-] [->]", FontSmall, BlueBrush, Width / 2 - 64, Height / 2, drawFormat);
                dc.DrawString("Disparo, Tecla: [X]", FontSmall, BlueBrush, Width / 2 - 64, Height / 2 + 60, drawFormat);
                dc.DrawString("Atrás", FontSmall, RedBrush, Width / 2 - 64, Height / 2 + 120, drawFormat);

            }
            else if (currMenu == 2)
            {
                dc.DrawString("Puntajes", FontBig, BlueBrush, Width / 2 - 80, Height / 2 - 100, drawFormat);             
                for(int i = 0; i < 3; i++)
                {
                    dc.DrawString((i + " - " + topUsers[i]), FontSmall, RedBrush, Width / 2 - 72, Height / 2 + i*20, drawFormat);
                    dc.DrawString(""+ topScore[i],FontSmall, RedBrush, Width / 2 + 128, Height / 2 + i*20, drawFormat);

                }
                dc.DrawString("Atrás", FontSmall, RedBrush, Width / 2 - 64, Height / 2 + 180, drawFormat);
            }
            else
            {

                dc.DrawImage(playerImg, new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Width, (int)player.Height));
               // dc.DrawImage(playerImg, GetBulletSpawnPoint);

                //dc.FillRectangle(whiteBrush, new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Width, (int)player.Height));
                dc.FillRectangle(yellowBrush, new Rectangle((int)pBullet.Position.X, (int)pBullet.Position.Y, (int)pBullet.Width, (int)pBullet.Height));

                foreach (var bullet in enemyBullets)
                {
                    dc.FillRectangle(whiteBrush, new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, (int)bullet.Width, (int)bullet.Height));
                }

                foreach (var enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if (enemy.points == 100)
                        {
                            dc.DrawImage(enemy1Img, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));
                            //dc.FillRectangle(greenBrush, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));

                        }
                        if (enemy.points == 200)
                        {
                            dc.DrawImage(enemy2Img, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));
                            //dc.FillRectangle(lightBlueBrush, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));

                        }
                        if (enemy.points == 300)
                        {
                            dc.DrawImage(enemy3Img, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));
                            //dc.FillRectangle(redBrush, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Width, (int)enemy.Height));

                        }

                    }

                }
                
            }

            dc.DrawString("Puntaje", FontSmall, RedBrush, 64, +32, drawFormat);
            dc.DrawString("" + puntaje, FontSmall, RedBrush, 64, +64, drawFormat);
            dc.DrawString("Puntaje Máximo", FontSmall, RedBrush, Width / 2 - 112, 32, drawFormat);
            dc.DrawString("" + topScore[0], FontSmall, RedBrush, Width / 2 - 112, 64, drawFormat);
            dc.DrawString("Vidas", FontSmall, RedBrush, 896, 32, drawFormat);
            dc.DrawString("" + vidas, FontSmall, RedBrush, 896, 64, drawFormat);
        }
       
        // "main"
        public Galaxian(int width, int height, System.Windows.Point playerSize, float playerSpd, int enemySize, int enemyOffset, System.Windows.Point originPos)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            path = path + "\\png\\";

            //Console.WriteLine($"The current directory is {path}");

            playerImg = new Bitmap(path + "playerShip.png");
            enemy1Img = new Bitmap(path + "ufoGreen.png");
            enemy2Img = new Bitmap(path + "ufoBlue.png");
            enemy3Img = new Bitmap(path + "ufoRed.png");
            /*
           playerImg = new Bitmap("jugador.png");
           nave1IMG = new Bitmap("nave1.png");
           nave2IMG = new Bitmap("nave2.png");
           nave3IMG = new Bitmap("nave3.png");
           playerIconIMG = new Bitmap("iconoNave.png");
           playerDeathIMG = new Bitmap("muerteJugador.png");
           naveDeathIMG = new Bitmap("muerteEnemigo.png");*/


            rand = new Random();
            Width = width;
            Height = height;

            estrellasSpeed = 0.1f;

            contadorEstrellas = 100;

            enemiesOrigin = originPos;
            this.enemySize = enemySize;
            inbetweenSpace = enemyOffset;
            originXOffset = (int)originPos.X;
            originSpeed = 0.13f;
            originSpots = 7;
            currOriginSpot = 6;
            originDirection = true;

            lunchEnemyTimer = 0;
            timeToLunchEnemy = 1300;

            eBulletWidth = 3;
            eBulletHeight = 9;
            eBulletSpeed = .6f;



           

            pfc.AddFontFile("ARCADE_N.TTF");
            FontSmall = new Font(pfc.Families[0], 12);
            FontBig = new Font(pfc.Families[0], 16);

            for (int i= 0; i < 3; i++)
            {
                topScore[i] = 0;
                topUsers[i] = "vacio";
            }
            
            string line;

            //valores 
            player = new Player(new Rect(width / 2, height - (playerSize.Y + 30), playerSize.X, playerSize.Y), playerSpd);
            pBullet = new Bullet(new Rect(GetBulletSpawnPoint().X, GetBulletSpawnPoint().Y, 3, 9), new Vector(0, -0.6f));

            pBulletActive = false;
           
            //enemybullets
            bulletsOrigin = new System.Windows.Point(-10, -10);        
           //for (int i = 0; i < 10; i++)
           // {
           //     enemyBullets[i] = new Bullet(new Rect(-10, -10, 3, 9), new Vector(0, -0.6));
           // }

            //leer puntaje máximo
            Console.WriteLine("Ingrese su nombre de usuario");
            try
            {
                StreamReader sr = new StreamReader("Puntaje.txt");

                line = sr.ReadLine();
                while (line != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        topUsers[i] = line;
                        line = sr.ReadLine();
                        topScore[i] = int.Parse(s: line);
                        line = sr.ReadLine();
                    }
          
                }
                user = Console.ReadLine();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }

            for(int i = 0; i < 16; i++)
            {
                estrellas[i] = new Rectangle(rand.Next(Width), rand.Next(Height), 2, 2);               
            }

        }

    }

} 
