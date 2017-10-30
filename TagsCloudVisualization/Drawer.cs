using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Drawer
    {
        public static void Draw(List<Rectangle> rectangles, Point cloudCenter, string fileName, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var g = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Blue);
            g.DrawEllipse(pen, cloudCenter.X-1, cloudCenter.Y-1, 2, 2);
            foreach (var rectangle in rectangles)
                g.DrawRectangle(pen, rectangle);

            g.Dispose();
            bitmap.Save(fileName);
        }
    }
}
