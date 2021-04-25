using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uEngine;
using System.Windows;

namespace Galaxian
{

    class GalaxianGame : uGame
    {
        private Galaxian gameData;

        private bool xKey;
        private bool leftKey;
        private bool rightKey;


        public GalaxianGame(int width, int height, int targetFPS) : base(width, height, targetFPS)
        {
            gameData = new Galaxian(width, height, new System.Windows.Point(30, 30), 0.3f, 30, 30, new System.Windows.Point(30, 150));

            leftKey = false;
            rightKey = true;
        }

        public override void GameUpdate()
        {
            
            if (rightKey && !leftKey)
            {
                gameData.MovePlayer(DeltaTime, true);
            }
            else if (leftKey && !rightKey)
            {
                gameData.MovePlayer(DeltaTime, false);
            }

            if (xKey)
            {
                gameData.PlayerShoot();
            }

            gameData.UpdatePBullet(DeltaTime);

            gameData.MoveOrigin(DeltaTime);
            gameData.MoveAllEnemies();


        }

        public override void ProcessInput()
        {
            xKey = InputManager.IsKeyPressed("X");

            leftKey = InputManager.IsKeyPressed("Left");
            rightKey = InputManager.IsKeyPressed("Right");

        }

        public override void Render(Graphics dc)
        {
            gameData.Paint(dc);
        }
    }
}
