using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.SpaceObjects;
using StarKnightsLibrary.SpaceObjects.Ships;
using Xunit;

namespace StarKnightsTests
{
    public class SpaceTests
    {
        [Fact]
        public void TestObjectsInBounds()
        {
            Space space = new Space(100, 100);
            SpaceObject obj = new Ship(0, 0, 0, 0, 0, 0, 0, false, new EmptyShipDecider(space), 0, "");

            Assert.True(space.IsInBounds(obj));
        }

        [Theory]
        [InlineData(50.1, 0)]
        [InlineData(0, 50.1)]
        [InlineData(-50.1, 0)]
        [InlineData(0, -50.1)]
        public void TestObjectOutOfBounds(float x, float y)
        {
            Space space = new Space(100, 100);
            SpaceObject obj = new Ship(x, y, 0, 0, 0, 0, 0, false, new EmptyShipDecider(space), 0, "");

            Assert.False(space.IsInBounds(obj));
        }

        [Fact]
        public void InBoundsObjectNotDestroyed()
        {
            //Arrange
            Space space = new Space(100, 100);
            int outOfBoundsLimit = space.OutOfBoundsLimit;
            SpaceObject spaceObject = space.AddNonPlayerShip<EmptyShipDecider>(0, 0, 0, 10, 10, 0);            

            //Act
            for(int i = 0; i < outOfBoundsLimit; i++)
            {
                space.Update();
            }

            //Assert
            Assert.False(spaceObject.Destroyed);
        }

        [Fact]
        public void OutOfBoundsObjectDestroyed()
        {
            //Arrange
            Space space = new Space(100, 100);
            int outOfBoundsLimit = space.OutOfBoundsLimit;
            SpaceObject spaceObject = space.AddNonPlayerShip<EmptyShipDecider>(50, 50, 0, 10, 10, 0);         

            //Act
            for (int i = 0; i < outOfBoundsLimit; i++)
            {
                space.Update();
            }

            //Assert
            Assert.True(spaceObject.Destroyed);
        }

        [Fact]
        public void ObjectLeavesAndComesBackNotDestroyed()
        {
            //Arrange
            Space space = new Space(100, 100);
            int outOfBoundsLimit = space.OutOfBoundsLimit;
            SpaceObject spaceObject = space.AddNonPlayerShip<EmptyShipDecider>(49, 49 + outOfBoundsLimit, 0, 10, 10, 0);
            spaceObject.Velocity.Current = 1;            

            //Act
            for (int i = 0; i < outOfBoundsLimit; i++)
            {
                space.Update();
            }

            //Assert
            Assert.False(spaceObject.Destroyed);
        }

        [Fact]
        public void ObjectLeavesAndComesBackLeavesAgainDestroyed()
        {
            //Arrange
            Space space = new Space(100, 100);
            int outOfBoundsLimit = space.OutOfBoundsLimit;
            SpaceObject spaceObject = space.AddNonPlayerShip<EmptyShipDecider>(49, 49 + outOfBoundsLimit, 0, 10, 10, 0);
            spaceObject.Velocity.Current = 1;            

            //Act
            for (int i = 0; i < outOfBoundsLimit; i++)
            {
                space.Update();
            }
            spaceObject.Orientation.Reverse();
            for (int i = 0; i < outOfBoundsLimit; i++)
            {
                Assert.False(spaceObject.Destroyed);
                space.Update();
            }

            //Assert
            Assert.True(spaceObject.Destroyed);
        }
    }
}
