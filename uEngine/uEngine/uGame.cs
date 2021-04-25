using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace uEngine
{
    public abstract class uGame
    {
        private uWindow Window;
        private int TargetFPS;

        public int Width { private set; get; }
        public int Height { private set; get; }

        public long DeltaTime { private set; get; }

        public uGame(int width, int height, int targetFPS)
        {
            Width = width;
            Height = height;
            Window = new uWindow(width, height);
            TargetFPS = targetFPS;
        }

        public void Start()
        {
            Window.Show();
            Thread thread = new Thread(GameLoop);
            thread.Start();
        }

        private void GameLoop()
        {
            long targetTime = 1000 / TargetFPS;

            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ProcessInput();
                GameUpdate();
                Render(Window.GetDrawing());
                Window.Render();
                sw.Stop();

                long time = sw.ElapsedMilliseconds;
                DeltaTime = time;
                int pause = (int)(targetTime - time);
                if (pause < 1)
                {
                    pause = 1;
                }
                DeltaTime += pause;

                Thread.Sleep(pause);


            }
        }

        public abstract void ProcessInput();
        public abstract void GameUpdate();
        public abstract void Render(Graphics dc);
        

    }
}
