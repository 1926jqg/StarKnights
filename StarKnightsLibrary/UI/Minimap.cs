using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.SpaceObjects;
using StarKnightsLibrary.SpaceObjects.Projectiles.Missiles;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.UtilityObjects;

namespace StarKnightsLibrary.UI
{
    public class Minimap
    {
        private const int BLIP_WIDTH = 8;
        private const int BLIP_HEIGHT = 8;

        private readonly Space _space;
        private readonly int _leftBorder;
        private readonly int _bottomBorder;
        private readonly int _width;
        private readonly int _height;
        

        public Minimap(Space space, int leftBorder, int bottomBorder, int width, int height)
        {
            _space = space;
            _leftBorder = leftBorder;
            _bottomBorder = bottomBorder;
            _width = width;
            _height = height;
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

            int xMin = _leftBorder;
            int yMax = viewport.Height - _height - _bottomBorder;

            spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), new Rectangle(xMin - BLIP_WIDTH / 2, yMax - BLIP_HEIGHT / 2, _width + BLIP_WIDTH, _height + BLIP_HEIGHT), Color.White * .5f);

            foreach (var spaceObject in _space.Objects)
            {
                if (!_space.IsInBounds(spaceObject))
                    continue;
                if (spaceObject.IsPointOfView)
                    continue;
                if (spaceObject is Ship)
                {
                    Ship ship = spaceObject as Ship;
                    DrawMinimapBogey(spriteBatch, contentContainer, ship, xMin, yMax);
                }
                else if (spaceObject is Missile)
                {
                    spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), GetMinimapRectangle(spaceObject, xMin, yMax, 1, 1), Color.Black);
                }
            }
            DrawMinimapBogey(spriteBatch, contentContainer, _space.PointOfViewObject, xMin, yMax);
        }

        private Rectangle GetMinimapRectangle(SpaceObject spaceObject, int xMin, int yMax, int width, int height)
        {
            var xScale = (spaceObject.Orientation.Position.X / _space.Width) * _width;
            var yScale = (spaceObject.Orientation.Position.Y / _space.Height) * _height;

            var xPos = xScale + xMin + _width / 2;
            var yPos = yScale + yMax + _height / 2;

            return new Rectangle((int)xPos, (int)yPos, width, height);
        }

        private void DrawMinimapBogey(SpriteBatch spriteBatch, IContentContainer contentContainer, Ship ship, int xMin, int yMax)
        {
            Color color;
            if (ship.IsPointOfView)
            {
                color = Color.Green;
            }
            else if (ship == _space.PointOfViewObject.Target)
            {
                color = Color.Yellow;
            }
            else if (ship.Team == _space.PointOfViewObject.Team)
            {
                color = Color.Blue;
            }
            else
            {
                color = Color.Red;
            }

            spriteBatch.Draw(contentContainer.LoadTexture("Bogey"), GetMinimapRectangle(ship, xMin, yMax, BLIP_WIDTH, BLIP_HEIGHT), null, color, ship.Orientation.Angle, new Vector2(BLIP_WIDTH, BLIP_HEIGHT), SpriteEffects.None, 1);
        }
    }
}
