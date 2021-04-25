using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uEngine.managers;


namespace uEngine
{
    public static class InputManager
    {
        public static Point MouseLocation()
        {
            return MouseManager.Get();
        }

        public static int MouseButton()
        {
            return MouseManager.MouseButton();
        }

        /// <summary>
        /// Para mayor información de los nombres de las teclas revisar:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=net-5.0 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyPressed(string key)
        {
            KeysConverter converter = new KeysConverter();
            Keys code = (Keys)converter.ConvertFromString(key);
            return KeyboardManager.IsPressed(code);
        }
    }
}
