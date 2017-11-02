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

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle size is not positive");

            var rectangle = FindLocation(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle FindLocation(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return PutFirstRectangle(rectangleSize, cloudCenter);
            
            var distance = 1;
            var rectangle = new Rectangle(new Point(0, 0), rectangleSize);

            while (true)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    rectangle.X = cloudCenter.X + Convert.ToInt32(distance * Math.Cos(i / Math.PI * 180));
                    rectangle.Y = cloudCenter.Y + Convert.ToInt32(distance * Math.Sin(i / Math.PI * 180));

                    if (!rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                        return rectangle;
                }
                distance += 1;
            }
        }

        private Rectangle PutFirstRectangle(Size rectangleSize, Point cloudCenter)
        {
            var shiftX = rectangleSize.Width / 2;
            var shiftY = rectangleSize.Height / 2;
            var top = new Point(cloudCenter.X - shiftX, cloudCenter.Y - shiftY);
            return new Rectangle(top, rectangleSize);
        }
    }
}
