using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaskanoidGame.model
{
    public class Raskanoid
    {
        private Rectangle Ball;
        private Rectangle Pad;
        private bool Started;
        private int dx;
        private int dy;

        private List<Rectangle> Bricks;

        private int Width { set; get; }
        private int Height { set; get; }

        private Image PadImage;
        private Image BrickImage;



        public Raskanoid(int width, int height)
        {
            Width = width;
            Height = height;

            PadImage = new Bitmap("D:\\Dropbox\\Universidad de Talca\\Ingeniería en Desarrollo de Videojuegos y Realidad Virtual\\Cursos\\2021 - 1\\Programación de Videojuegos 2D\\Proyectos\\uEngine\\png\\paddleRed.png");
            BrickImage = new Bitmap("D:\\Dropbox\\Universidad de Talca\\Ingeniería en Desarrollo de Videojuegos y Realidad Virtual\\Cursos\\2021 - 1\\Programación de Videojuegos 2D\\Proyectos\\uEngine\\png\\element_green_rectangle.png");

            Initialize();

        }

        private void Initialize()
        {
            Started = false;
            dx = -1;
            dy = -1;

            Pad = new Rectangle(0, 680, 100, 20);
            Ball = new Rectangle(Width / 2, Pad.Y - 20, 20, 20);

            Bricks = new List<Rectangle>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Bricks.Add(new Rectangle(100 + 105*i, 50 + 25 * j, 100, 20));
                }
            }
            

        }

        public void Start()
        {
            Started = true;
        }

        public void Paint(Graphics dc)
        { 
            //dc.FillRectangle(System.Drawing.Brushes.White, Pad.Parse());
            dc.DrawImage( PadImage , Pad.Parse());

            dc.FillRectangle(System.Drawing.Brushes.White, Ball.Parse());

            foreach(Rectangle brick in Bricks)
            {
                //dc.FillRectangle(System.Drawing.Brushes.White, brick.Parse());
                dc.DrawImage(BrickImage, brick.Parse());
            }
        }

        public void Update(Point mouseLocation, int deltaTime, bool LeftKey, bool RightKey)
        {
            Console.WriteLine(deltaTime);
            
            //UpdatePad(mouseLocation);

            if(LeftKey)
            {
                Pad.X -= deltaTime;
            }

            if(RightKey)
            {
                Pad.X += deltaTime;
            }



            
            UpdateBall(deltaTime);
            UpdateBricks();
            
        }

        private void UpdateBricks()
        {
            List<Rectangle> toRemove = new List<Rectangle>();
            foreach (Rectangle brick in Bricks)
            {
                if(Ball.Intersects(brick))
                {
                    toRemove.Add(brick);
                }
            }

            foreach (Rectangle brick in toRemove)
            {
                Bricks.Remove(brick);
            }

        }

        private void UpdateBall(int deltaTime)
        {
            if (!Started)
            {
                Ball.X = Pad.X + Pad.Width / 2 - Ball.Width / 2;
                Ball.Y = Pad.Y - 20;
            }
            else
            {
                Ball.X += dx * deltaTime;
                Ball.Y += dy * deltaTime;
                if (Ball.X <= 0 || Ball.X >= Width - 20)
                {
                    dx *= -1;
                }

                if (Ball.Y <= 0 || Ball.Y >= Height - 20)
                {
                    dy *= -1;
                }

                if (Ball.Y >= Height - 20)
                {
                    Started = false;
                    dx = -1;
                    dy = -1;
                }
            }
        }

        private void UpdatePad(Point mouseLocation)
        {
            Pad.X = mouseLocation.X - 50;

            if (Pad.X < 0)
            {
                Pad.X = 0;
            }
            else if (Pad.X + 100 > Width)
            {
                Pad.X = Width - 100;
            }
        }
    }
}
