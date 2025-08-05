using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Drawing;

namespace PelliP3
{
    public class WindowUtils
    {
        public static void drawTextCentered(Label text, Size ClientSize)
        {
            Size textSize = TextRenderer.MeasureText(text.Text, text.Font);
            text.Location = new Point((ClientSize.Width - textSize.Width) / 2, text.Location.Y);
        }
    }
}
