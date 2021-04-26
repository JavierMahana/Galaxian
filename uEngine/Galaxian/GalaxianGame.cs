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
        private int leftMouse;
        private System.Drawing.Point mousePosition;
        private bool Gamescene;
        private int clickDelay = 0;

        public GalaxianGame(int width, int height, int targetFPS) : base(width, height, targetFPS)
        {
            gameData = new Galaxian(width, height, new System.Windows.Point(30, 30), 0.3f, 30, 30, new System.Windows.Point(30, 150));

            leftKey = false;
            rightKey = true;
            Gamescene = false;
            
        }

        public override void GameUpdate()
        {
            gameData.MoveEstrellas(DeltaTime);
            // menu
            if (!Gamescene)
            {

                if(leftMouse != -1 && clickDelay == 0)
                {
                    clickDelay = 8;
                    Gamescene = gameData.MenuOptions(mousePosition);
                    gameData.newScene(true); 
                    
                }
               
            }
            // escena de juego
            else 
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

                //gameData.MoveEstrellas(DeltaTime);

                gameData.UpdatePBullet(DeltaTime);
                gameData.MoveOrigin(DeltaTime);
                gameData.MoveAllEnemies(DeltaTime);

                gameData.MoveEnemyBullets(DeltaTime);


                gameData.EnemycollisionCheck();
                gameData.EnemyBulletCollisionCheck();

                gameData.EnemyTryToShoot();
                //gameData.EnemyShooter(DeltaTime);
                    
                gameData.TryLunchEnemyAttack(DeltaTime);

                Gamescene = gameData.VictoryCondition();
            }
            if (clickDelay > 0)
            {
                clickDelay--;
            }

        }

        public override void ProcessInput()
        {
            xKey = InputManager.IsKeyPressed("X");
            leftKey = InputManager.IsKeyPressed("Left");
            rightKey = InputManager.IsKeyPressed("Right");
            leftMouse = InputManager.MouseButton();
            mousePosition = InputManager.MouseLocation();

        }

        public override void Render(Graphics dc)
        {
            gameData.Paint(dc);
        }
    }
}
