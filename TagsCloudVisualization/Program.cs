using System;
using System.Drawing;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point cloudCenter;
        private List<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (Rectangles.Count == 0)
            {
                var shift = new Point(rectangleSize.Width/2, rectangleSize.Height/2);
                var top = new Point(cloudCenter.X - shift.X, cloudCenter.Y - shift.Y);
                Rectangles.Add(new Rectangle(top, rectangleSize));
                return new Rectangle(top, rectangleSize);
            }
            return FindLocation(rectangleSize);
        }

        private Rectangle FindLocation(Size rectangleSize)
        {
            return new Rectangle();
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_Test
    {
        [Test]
        public void PutNextRectangle_WhenRectangleIsFirst()
        {
            var cloud = new CircularCloudLayouter(new Point(5,5));
            var rectangle = cloud.PutNextRectangle(new Size(2, 2));
            rectangle.Size.ShouldBeEquivalentTo(new Size(2, 2));
            rectangle.X.Should().Be(4);
            rectangle.Y.Should().Be(4);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}