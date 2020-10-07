using StarKnightsLibrary.UtilityObjects.Collision;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Xunit;

namespace StarKnightsTests
{
    public class CollisionTests
    {
        [Theory]
        [InlineData(10, 0, 0, 0, 0)]
        [InlineData(10, 0, 0, 5, 5)]
        [InlineData(10, 0, 0, -5, -5)]
        [InlineData(10, 0, 0, 9.9, 0)]
        [InlineData(10, 0, 0, 0, 9.9)]
        public void RectanglesCollideTest(int size, float x1, float y1, float x2, float y2)
        {
            //Arrange
            var r1 = new Rectangle(size, size, x1, y1);
            var r2 = new Rectangle(size, size, x2, y2);

            //Act
            var collision = r1.CollidesWith(r2);

            //Assert
            Assert.True(collision);
        }

        [Theory]
        [InlineData(10, 0, 0, 100, 100)]
        [InlineData(10, 0, 0, 0, -10)]
        [InlineData(10, 0, 0, -10, 0)]
        [InlineData(10, 0, 0, 10, 0)]
        [InlineData(10, 0, 0, 0, 10)]
        public void RectanglesNotCollideTest(int size, float x1, float y1, float x2, float y2)
        {
            //Arrange
            var r1 = new Rectangle(size, size, x1, y1);
            var r2 = new Rectangle(size, size, x2, y2);

            //Act
            var collision = r1.CollidesWith(r2);

            //Assert
            Assert.False(collision);
        }
        [Theory]
        [InlineData(10, 0, 0, 0, 5, -5, MathHelper.PiOver2)]
        [InlineData(10, 0, 0, 0, 0, -9.9, MathHelper.PiOver2)]
        [InlineData(10, 0, 0, 0, 9.9, 0, MathHelper.PiOver2)]
        public void LinesCollideTest(int length, float x1, float y1, float a1, float x2, float y2, float a2)
        {
            //Arrange
            var l1 = new Line(length, x1, y1, a1);
            var l2 = new Line(length, x2, y2, a2);

            //Act
            var collision = l1.CollidesWith(l2);

            //Assert
            Assert.True(collision);
        }

        [Theory]
        [InlineData(10, 0, 0, 0, 0, 1, 0)]
        [InlineData(10, 0, 0, 0, 100, 100, 0)]
        public void LinesNotCollideTest(int length, float x1, float y1, float a1, float x2, float y2, float a2)
        {
            //Arrange
            var l1 = new Line(length, x1, y1, a1);
            var l2 = new Line(length, x2, y2, a2);

            //Act
            var collision = l1.CollidesWith(l2);

            //Assert
            Assert.False(collision);
        }

        [Theory]
        [InlineData(10, 0, 0, 5, -5, MathHelper.PiOver2)]
        [InlineData(10, 0, 0, 0, -9.9, MathHelper.PiOver2)]
        [InlineData(10, 0, 0, 9.9, 0, MathHelper.PiOver2)]
        public void LineRectangleCollideTest(int size, float rx, float ry, float lx, float ly, float la)
        {
            //Arrange
            var r = new Rectangle(size, size, rx, ry);
            var l = new Line(size, lx, ly, la);

            //Act
            var collision1 = r.CollidesWith(l);
            var collision2 = l.CollidesWith(r);

            //Assert
            Assert.True(collision1);
            Assert.True(collision2);
        }

        [Theory]
        [InlineData(10, 0, 0, -1, -1, 0)]
        [InlineData(10, 0, 0, 100, 100, 0)]
        public void LineRectangleNotCollideTest(int size, float rx, float ry, float lx, float ly, float la)
        {
            //Arrange
            var r = new Rectangle(size, size, rx, ry);
            var l = new Line(size, lx, ly, la);

            //Act
            var collision1 = r.CollidesWith(l);
            var collision2 = l.CollidesWith(r);

            //Assert
            Assert.False(collision1);
            Assert.False(collision2);
        }

        [Fact]
        public void SpatialHashMapGetNearTest()
        {
            //Arrange
            var spatialHashMap = new SpatialHashMap<ICollidable>(1000, 1000, 10);
            var r1 = new Rectangle(10, 10, 0, 0);
            var r2 = new Rectangle(10, 10, 5, 5);
            spatialHashMap.RegisterCollidable(r1);
            spatialHashMap.RegisterCollidable(r2);

            //Act
            var neighbors = spatialHashMap.GetNearby(r1);

            //Assert
            Assert.Contains(r2, neighbors);
        }

        [Fact]
        public void SpatialHashMapNotGetNearTest()
        {
            //Arrange
            var spatialHashMap = new SpatialHashMap<ICollidable>(100, 100, 20);
            var r1 = new Rectangle(10, 10, 0, 0);
            var r2 = new Rectangle(10, 10, -50, 39);
            var r3 = new Rectangle(10, 10, 39, 39);
            var r4 = new Rectangle(10, 10, -50, -50);
            var r5 = new Rectangle(10, 10, 39, -50);
            spatialHashMap.RegisterCollidable(r1);
            spatialHashMap.RegisterCollidable(r2);
            spatialHashMap.RegisterCollidable(r3);
            spatialHashMap.RegisterCollidable(r4);
            spatialHashMap.RegisterCollidable(r5);
             
            //Act
            var neighbors = spatialHashMap.GetNearby(r1);

            //Assert
            Assert.Empty(neighbors);
        }

        [Fact]
        public void SpatialHashClearTest()
        {
            //Arrange
            var spatialHashMap = new SpatialHashMap<ICollidable>(1000, 1000, 10);
            var r1 = new Rectangle(10, 10, 0, 0);
            var r2 = new Rectangle(10, 10, 5, 5);
            spatialHashMap.RegisterCollidable(r1);
            spatialHashMap.RegisterCollidable(r2);

            //Act
            spatialHashMap.Clear();
            var neighbors = spatialHashMap.GetNearby(r1);

            //Assert
            Assert.Empty(neighbors);
        }

        [Fact]
        public void SpatialHashLineTest()
        {
            //Arrange
            var spatialHashMap = new SpatialHashMap<ICollidable>(100, 100, 10);
            var l = new Line(30, 0, 0);
            var r = new Rectangle(10, 10, 0, -20);
            spatialHashMap.RegisterCollidable(l);
            spatialHashMap.RegisterCollidable(r);

            //Act
            var neighbors = spatialHashMap.GetNearby(l);

            //Assert
            Assert.Empty(neighbors);
        }
    }
}
