using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

using uEngine.managers;

namespace uEngine
{
    class uWindow : Form
    {
        private BufferedGraphicsContext GraphicManager;
        private BufferedGraphics ManagedBackBuffer;

        public uWindow(int width, int height)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new System.Drawing.Size(width, height);

            GraphicManager = BufferedGraphicsManager.Current;
            GraphicManager.MaximumBuffer = new System.Drawing.Size(width + 1, height + 1);
            ManagedBackBuffer = GraphicManager.Allocate(CreateGraphics(), ClientRectangle);

            MouseMove += CustomMouseMove;
            MouseDown += CustomMouseDown;
            MouseUp += CustomMouseUp;

            KeyDown += CustomKeyDown;
            KeyUp += CustomKeyUp;
        }
        
        public Graphics GetDrawing()
        {

            //Graphics g = CreateGraphics();
            //g.Clear(System.Drawing.Color.Black);
            //return g;

            ManagedBackBuffer.Graphics.Clear(System.Drawing.Color.Black);
            return ManagedBackBuffer.Graphics;
        }

        public void Render()
        {
            ManagedBackBuffer.Render();
        }



        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
        }

        private void CustomMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseManager.UpdateLocation(e.X, e.Y);
        }

        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left: MouseManager.UpdateMouseButton(1); break;
                case MouseButtons.Right: MouseManager.UpdateMouseButton(2); break;
                case MouseButtons.Middle: MouseManager.UpdateMouseButton(3); break;
                default: MouseManager.UpdateMouseButton(-1); break;
            }
        }

        private void CustomMouseUp(object sender, MouseEventArgs e)
        {
            MouseManager.UpdateMouseButton(-1);
        }

        private void CustomKeyDown(object sender, KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            KeyboardManager.Down(code);
        }

        private void CustomKeyUp(object sender, KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            KeyboardManager.Up(code);
        }
    }
}
