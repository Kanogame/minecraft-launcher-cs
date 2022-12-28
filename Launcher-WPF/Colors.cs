using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Launcher_WPF
{
    public class Colors
    {
        private static Color[,] colors = new Color[2, 2] {
            {
                Color.FromRgb(91, 195, 255), 
                Color.FromRgb(58, 160, 255)
            },
            {
                Color.FromRgb(91, 195, 255),
                Color.FromRgb(58, 160, 255)
            },
        };
        
        private Random rd = new Random();

        public Color[] GetColor()
        {
            int rand = rd.Next(2);
            var col1 = colors[rand, 0];
            var col2 = colors[rand, 1];
            return new Color[2]
            {
                col1,
                col2
            };
        }
    }
}
