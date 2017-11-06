using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point cloudCenter;
        private List<Rectangle> rectangles;
        private int distance;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle size is not positive");

            var rectangle = new Rectangle(FindLocation(rectangleSize), rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Point FindLocation(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {
                var shiftX = rectangleSize.Width / 2;
                var shiftY = rectangleSize.Height / 2;
                return new Point(cloudCenter.X - shiftX, cloudCenter.Y - shiftY);
            }

            var points = GetTop().GetEnumerator();
            points.MoveNext();
            var rectangle = new Rectangle(points.Current, rectangleSize);
            while (rectangles.Any(rect => rect.IntersectsWith(rectangle)))
            {
                points.MoveNext();
                rectangle.X = points.Current.X;
                rectangle.Y = points.Current.Y;
            }
            return points.Current;
        }

        public IEnumerable<Point> GetTop()
        {
            while (true)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    yield return
                        new Point(
                            cloudCenter.X + Convert.ToInt32(distance * Math.Cos(i / Math.PI * 180)),
                            cloudCenter.Y + Convert.ToInt32(distance * Math.Sin(i / Math.PI * 180))
                        );
                }
                distance += 1;
            }
        }
    }
}
