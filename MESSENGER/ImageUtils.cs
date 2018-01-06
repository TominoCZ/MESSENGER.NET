using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESSENGER
{
    class ImageUtils
    {
        public static Image ResizeAndCrop(Image img, int w, int h)
        {
            Bitmap cropped = new Bitmap(w, h);

            double d = (double)h / img.Height;

            Image i = new Bitmap(img, (int)Math.Round(img.Width * d), (int)Math.Round(img.Height * d));

            Graphics g = Graphics.FromImage(cropped);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(i, new PointF(w / 2f - i.Width / 2f, h / 2f - i.Height / 2f));

            return cropped;
        }

        public static byte[] GetBytes(Image img)
        {
            if (img == null)
                return null;

            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image GetImage(byte[] img)
        {
            if (img == null)
                return null;

            using (var ms = new MemoryStream(img))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
