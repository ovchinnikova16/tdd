using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                throw new ArgumentException();

            if (rectangles.Count == 0)
            {
                var shift = new Point(rectangleSize.Width/2, rectangleSize.Height/2);
                var top = new Point(cloudCenter.X - shift.X, cloudCenter.Y - shift.Y);

                rectangles.Add(new Rectangle(top, rectangleSize));
                return new Rectangle(top, rectangleSize);
            }

            var rectangle = FindLocation(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle FindLocation(Size rectangleSize)
        {
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
    }

    [TestFixture]
    public class CircularCloudLayouter_Test
    { 
        [Test]
        public void PutNextRectangle_Throw_ThenNotPozitiveSize()
        {
            var cloud = new CircularCloudLayouter(new Point(5, 5));
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(new Size(-1, 1)));
        }

        [TestCase(2, 2, 4, 4)]
        [TestCase(3, 3, 4, 4)]
        public void PutNextRectangle_FirstRectangle_InCloudCenter(int width, int height, int topX, int topY)
        {
            var cloud = new CircularCloudLayouter(new Point(5, 5));

            var rectangle = cloud.PutNextRectangle(new Size(width, height));

            rectangle.Size.ShouldBeEquivalentTo(new Size(width, height));
            rectangle.X.Should().Be(topX);
            rectangle.Y.Should().Be(topY);
        }

        [TestCase(4, 4, 3, 3)]
        [TestCase(3, 3, 4, 4)]
        public void PutNextRectangle_AddTwoRectangles_InCloud(int size1X, int size1Y, int size2X, int size2Y)
        {
            var cloud = new CircularCloudLayouter(new Point(10, 10));

            cloud.PutNextRectangle(new Size(size1X, size1Y));
            var rectangle = cloud.PutNextRectangle(new Size(size2X, size2Y));

            rectangle.Size.ShouldBeEquivalentTo(new Size(size2X, size2Y));
        }

        [Test]
        public void PutNextRectangle_AddSeweralRectangles_InCloud()
        {
            var cloud = new CircularCloudLayouter(new Point(10, 10));

            cloud.PutNextRectangle(new Size(2, 2));
            cloud.PutNextRectangle(new Size(3, 3));
            var rectangle = cloud.PutNextRectangle(new Size(2, 2));

            rectangle.Size.ShouldBeEquivalentTo(new Size(2, 2));
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}