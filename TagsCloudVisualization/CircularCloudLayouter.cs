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

                // стоит писать сообщения в исключениях, чтоб (когда их, например, несколько) было сразу понятно, что произошло

                throw new ArgumentException();

            if (rectangles.Count == 0)
            {
                var shift = new Point(rectangleSize.Width / 2, rectangleSize.Height / 2);
                var top = new Point(cloudCenter.X - shift.X, cloudCenter.Y - shift.Y);

                // метод обманывает, т.к. кладет на layouter он один прямоугольник, а ссылку возвращает на другой, хоть и эквивалентный

                rectangles.Add(new Rectangle(top, rectangleSize));
                return new Rectangle(top, rectangleSize);
            }

            // вся логика из ветки if выше - в общем-то, тоже часть задачи метода FindLocation

            var rectangle = FindLocation(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }


        // FindLocation, конечно, работает неоптимально: 
        //    чтобы найти положение очередного прямоугольника, ты тратишь около линии на то, чтобы пройти по координатам всех уже найденных прямоугольников
        //    и проверить, были ли уже такие прямоугольники
        // новую координату можно получить за константу, если хранить где-то текущее состояние - координату для следующего прямоугольника

        // эту логику состояния, к слову, лучше вынести в отдельный класс, т.к. это отдельная работа, отдельная ответственность - про SRP у вас должен быть следующий блок :)

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

                    // Any - это линия O(n).
                    // чтобы узнать, как работает тот или иной метод из дотнета, можешь поставить себе решарпер, если еще не стоит
                    //   и по ctrl + click или ctrl + b решарпер будет декомпилировать исходники
                    //   ничего особенно сложного под капотом у линку-методов нет, можно без особого труда прочесть, чтобы оценить алгоритмическую сложность

                    if (!rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                        return rectangle;
                }
                distance += 1;
            }
        }
    }

    // тесты лучше выносить в отдельный файл хотя бы. обычно их выносят в отдельную сборку
    [TestFixture]
    // CircularCloudLayouter_Should
    public class CircularCloudLayouter_Test
    {
        // запариваться за то, что названия тестов длинные, не стоит, здесь важнее длины то, что можно было понять, что он проверяет, просто прочитав название
        // When_PutNextRectangle_Throw_IfNotPozitiveSize
        [Test]
        public void PutNextRectangle_Throw_ThenNotPozitiveSize()
        {
            // создать общий для всех тестов CircularCloudLayouter можно в методе с атрибутом [SetUp], поместив объект в поле этого класса
            var cloud = new CircularCloudLayouter(new Point(5, 5));
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(new Size(-1, 1)));
        }

        [TestCase(2, 2, 4, 4)]
        [TestCase(3, 3, 4, 4)]
        // Put_FirstRectangle_InCloudCenter
        public void PutNextRectangle_FirstRectangle_InCloudCenter(int width, int height, int topX, int topY)
        {
            var cloud = new CircularCloudLayouter(new Point(5, 5));

            var rectangle = cloud.PutNextRectangle(new Size(width, height)); // не стоит создавать два одинаковых объекта без особой цели

            rectangle.Size.ShouldBeEquivalentTo(new Size(width, height));
            rectangle.X.Should().Be(topX);
            rectangle.Y.Should().Be(topY);
        }

        [TestCase(4, 4, 3, 3)]
        [TestCase(3, 3, 4, 4)]
        // тут вот кажется, что тест проверяет две вещи и по названию непонятна ни одна из них )
        public void PutNextRectangle_AddTwoRectangles_InCloud(int size1X, int size1Y, int size2X, int size2Y) // x1, y1, x2, y2
        {
            var cloud = new CircularCloudLayouter(new Point(10, 10));

            var firstRectangle = cloud.PutNextRectangle(new Size(size1X, size1Y));
            var secondRectangle = cloud.PutNextRectangle(new Size(size2X, size2Y));

            secondRectangle.Size.ShouldBeEquivalentTo(new Size(size2X, size2Y));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        // кажется, что понятно, что тут проверяется, но с названием все еще хуже, чем в предыдущем тесте
        // другой минус - кажется, что тест слишком сложный и то, что он проверяет, можно написать как-то попроще
        public void PutNextRectangle_AddSeveralRectangles_InCloud()
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
