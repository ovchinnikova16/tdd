using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloudFirst = new CircularCloudLayouter(new Point(250, 250));
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
                rectangles.Add(cloudFirst.PutNextRectangle(new Size(10, 10)));
            Drawer.Draw(rectangles, new Point(250, 250), "cloud1.bmp", new Size(500, 500));

            var cloudSecond = new CircularCloudLayouter(new Point(250, 250));
            rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                var size = new Size(rnd.Next(1, 5) * 10, rnd.Next(1, 5) * 10);
                rectangles.Add(cloudSecond.PutNextRectangle(size));
            }
            Drawer.Draw(rectangles, new Point(250, 250), "cloud2.bmp", new Size(500, 500));
        }
    }
}