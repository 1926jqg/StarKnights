using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.Transmissions;
using StarKnightsLibrary.UtilityObjects;
using System;
using System.Collections.Generic;

namespace StarKnightsLibrary.UnitTests.TestDoubles.Fakes
{
    public class FakeSpaceScene : ISpaceScene
    {
        private readonly Space _space;

        public IEnumerable<Ship> Ships => _space.Ships;

        public ulong Ticks => _space.Ticks;

        public bool IsOver => false;

        public IScene Next => throw new NotImplementedException();

        public FakeSpaceScene(Space space)
        {
            _space = space;
        }

        public void AddTransmission(ITransmission transmission, ulong duration)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            _space.Update();
        }
    }
}
