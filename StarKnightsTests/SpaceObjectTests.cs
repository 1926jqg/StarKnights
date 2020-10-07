using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.SpaceObjects;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.SpaceObjects.Ships.Deciders;
using StarKnightsLibrary.UtilityObjects.Extensions;
using Xunit;

namespace StarKnightsTests
{
    public class SpaceObjectTests
    {
        [Fact]
        public void TestShipInBounds()
        {
            Space space = new Space(100, 100);
            Ship ship = new Ship(0, 0, 0, 0, 0, 0, 0, false, new EmptyShipDecider(space), 0, "");

            Assert.True(ship.IsInBounds());
        }

        [Theory]
        [InlineData(50.1, 0)]
        [InlineData(0, 50.1)]
        [InlineData(-50.1, 0)]
        [InlineData(0, -50.1)]
        public void TestShipOutOfBounds(float x, float y)
        {
            Space space = new Space(100, 100);
            Ship ship = new Ship(x, y, 0, 0, 0, 0, 0, false, new EmptyShipDecider(space), 0, "");

            Assert.False(ship.IsInBounds());
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(0, 10)]
        [InlineData(-10, 0)]
        [InlineData(0, -10)]
        public void TestBoundaryDeciderReturnsToBounds(int x, int y)
        {
            //Arrange
            Space space = new Space(100, 100, int.MaxValue);
            SpaceObject spaceObject = space.AddNonPlayerShip<BoundaryDecider>(x + (x > 0 ? space.XMax : space.XMin), y + (y > 0 ? space.YMax : space.YMin), 0, 10, 10, 0);

            //Act
            for (int i = 0; i < 500; i++)
            {
                space.Update();
            }

            //Assert
            Assert.True(space.IsInBounds(spaceObject));
        }

        [Fact]
        public void IntelligentFollowDeciderTest()
        {
            //Arrange
            Space space = new Space(1000, 1000, int.MaxValue);
            Ship npc = space.AddNonPlayerShip<IntelligentFollowDecider>(600, 500, 0, 10, 10, 0);            

            //Act
            for (int i = 0; i < 200; i++)
            {
                space.Update();
            }
            Assert.True(space.IsInBounds(npc));
            Assert.Equal(0, npc.Velocity.Current);

            Ship playerShip = space.AddPlayerShip(0, 0, 0, 0, 0, 0);
            npc.Target = playerShip;
            for(int i = 0; i < 25; i++)
            {
                space.Update();
            }

            //Assert
            Assert.True(npc.Velocity.Current > 0);
        }

        [Fact]
        public void ChooseTeamTargetTest()
        {
            //Arrange
            Space space = new Space(2048, 2048);
            Ship ship1 = space.AddNonPlayerShip<TeamTargetDecider>(500, 500, 0, 10, 10, 0);
            Ship ship2 = space.AddNonPlayerShip<TeamTargetDecider>(1000, 1000, 0, 10, 10, 1);

            //Act
            space.Update();

            //Assert
            Assert.NotEqual(ship1.Team, ship2.Team);
            Assert.Equal(ship1.Target, ship2);
            Assert.Equal(ship2.Target, ship1);
        }

        [Fact]
        public void ChooseTeamTargetNoOtherTeamTest()
        {
            //Arrange
            Space space = new Space(2048, 2048);
            Ship ship1 = space.AddNonPlayerShip<TeamTargetDecider>(500, 500, 0, 10, 10, 0);
            Ship ship2 = space.AddNonPlayerShip<TeamTargetDecider>(1000, 1000, 0, 10, 10, 0);

            //Act
            space.Update();

            //Assert
            Assert.Null(ship1.Target);
            Assert.Null(ship2.Target);
        } 

        [Fact]
        public void CompositeDeciderTest()
        {
            //Arrange
            Space space = new Space(2048, 2048);
            Ship ship1 = space.AddNonPlayerShip<TeamTargetDecider>(500, 0, 0, 10, 10, 0);
            Ship ship2 = space.AddNonPlayerShip<IntelligentFollowDecider, TeamTargetDecider>(0, 500, 0, 10, 10, 1);

            double distance = ship1.Orientation.Position.Distance(ship2.Orientation.Position);

            //Act
            for(int i = 0; i < 50; i++)
            {
                space.Update();
            }
            double newDistance = ship1.Orientation.Position.Distance(ship2.Orientation.Position);

            //Assert
            Assert.True(distance > newDistance);
        }
    }
}
