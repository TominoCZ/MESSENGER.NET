using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESSENGER
{
    class DrawingHelper
    {
        public static void enableSmooth(Graphics g, bool smooth)
        {
            if (smooth)
                smoothRendering(g);
            else
                sharpRendering(g);
        }

        private static void smoothRendering(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.InterpolationMode = InterpolationMode.Bicubic;
        }

        private static void sharpRendering(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.TextRenderingHint = TextRenderingHint.SystemDefault;
            g.InterpolationMode = InterpolationMode.Bicubic;
        }
    }
}
