using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArdClock.src.UIGenerate
{
    static class UIGenerateHelping
    {
        static public GridSplitter NewGridSplitter(int width, Brush bgColor)
        {
            GridSplitter gs = new GridSplitter
            {
                Width = width,
                Background = bgColor
            };

            return gs;
        }
    }
}
