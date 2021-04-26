using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Galaxian
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            GalaxianGame game = new GalaxianGame(1024, 768, 60);
            game.Start();

            Application.Run();

        }
    }
}
