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
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void When_PutNextRectangle_Throw_IfNotPozitiveSize()
        {
            var cloud = new CircularCloudLayouter(new Point(5, 5));
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(new Size(-1, 1)));
        }

        [TestCase(2, 2, 4, 4)]
        [TestCase(3, 3, 4, 4)]
        public void Put_FirstRectangle_InCloudCenter(int width, int height, int topX, int topY)
        {
            var cloud = new CircularCloudLayouter(new Point(5, 5));

            var rectangle = cloud.PutNextRectangle(new Size(width, height));

            rectangle.Size.ShouldBeEquivalentTo(new Size(width, height));
            rectangle.X.Should().Be(topX);
            rectangle.Y.Should().Be(topY);
        }

        [TestCase(4, 4, 3, 3)]
        [TestCase(3, 3, 4, 4)]
        public void Put_TwoRectangles_ShouldNotIntersect(int size1X, int size1Y, int size2X, int size2Y)
        {
            var cloud = new CircularCloudLayouter(new Point(10, 10));

            var firstRectangle = cloud.PutNextRectangle(new Size(size1X, size1Y));
            var secondRectangle = cloud.PutNextRectangle(new Size(size2X, size2Y));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void Put_SeveralRectangles_ShouldBeNotFarFromCloudCenter()
        {
            var cloud = new CircularCloudLayouter(new Point(20, 20));
            var rectangles = new List<Rectangle>();

            for (int i = 0; i < 5; i++)
                rectangles.Add(cloud.PutNextRectangle(new Size(2, 2)));

            foreach (var rectangle in rectangles)
            {
                rectangle.X.Should().BeGreaterThan(10);
                rectangle.Y.Should().BeGreaterThan(10);
                rectangle.X.Should().BeLessThan(30);
                rectangle.Y.Should().BeLessThan(30);
            }
        }
    }
}
