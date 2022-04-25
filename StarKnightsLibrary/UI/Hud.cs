using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.SpaceObjects;
using StarKnightsLibrary.SpaceObjects.Projectiles.Missiles;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.Transmissions;
using StarKnightsLibrary.UtilityObjects;
using System.Text;

namespace StarKnightsLibrary.UI
{
    public class Hud
    {
        public ITransmissionManager TransmissionManager { get; }
        private readonly Space _space;
        private Panel _infoPanel;
        private readonly int _height;
        private readonly int _width;
        private readonly Paragraph _paragraph;

        public Hud(Space space, int width, int height)
        {
            _width = width;
            _height = height;
            TransmissionManager = new TransmissionManager(width - height - 28 - 250, height - 2, 140);
            _space = space;
            _paragraph = new RichParagraph("", scale: 1f, offset: new Vector2(-10, -10), size: new Vector2(250, 200))
            {
                Padding = new Vector2(0, 0)
            };
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer, UserInterface ui)
        {
            TransmissionManager.Draw(ui, contentContainer);
            DrawInfo(ui);
            DrawMinimap(spriteBatch, contentContainer, 10, 10, _height, _height);
        }

        private void DrawInfo(UserInterface ui)
        {
            var playerShip = _space.PointOfViewObject;
            var target = playerShip.Target as Ship;

            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Shields:        {0}", playerShip.ShieldGenerator.ShieldPercentage.ToString("0.00%")));
            builder.AppendLine(string.Format("Hull:           {0}", playerShip.Hull));
            builder.AppendLine(string.Format("Target Shields: {0}", target?.ShieldGenerator.ShieldPercentage.ToString("0.00%")));
            builder.AppendLine(string.Format("Target Hull:    {0}", target?.Hull));
            builder.AppendLine(string.Format("Engines:        {{{{GREEN}}}}{0}{{{{DEFAULT}}}}", new string('o', playerShip.Engine.Power)));
            builder.AppendLine(string.Format("Shield :        {{{{GREEN}}}}{0}{{{{DEFAULT}}}}", new string('o', playerShip.ShieldGenerator.Power)));
            builder.AppendLine(string.Format("Weapons:        {{{{GREEN}}}}{0}{{{{DEFAULT}}}}", new string('o', playerShip.WeaponSystem.Power)));
            builder.AppendLine(string.Format("In Bounds:      {0}", _space.IsInBounds(playerShip)));

            if (_infoPanel == null)
            {
                const int panelWidth = 250;
                const int panelHeight = 200;
                _infoPanel = new Panel(
                    new Vector2(panelWidth, panelHeight + 9),
                    anchor: Anchor.TopLeft,
                    offset: new Vector2(_width - panelWidth - 10, ui.ScreenHeight - panelHeight - 13))
                {
                    Opacity = 128
                };
                ui.AddEntity(_infoPanel);
                _infoPanel.AddChild(_paragraph);
            }

            _paragraph.Text = builder.ToString();
        }

        private const int BLIP_WIDTH = 8;
        private const int BLIP_HEIGHT = 8;

        private void DrawMinimap(SpriteBatch spriteBatch, IContentContainer contentContainer, int leftBorder, int bottomBorder, int width, int height)
        {
            Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

            int xMin = leftBorder;
            int yMax = viewport.Height - height - bottomBorder;

            spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), new Rectangle(xMin - BLIP_WIDTH / 2, yMax - BLIP_HEIGHT / 2, width + BLIP_WIDTH, height + BLIP_HEIGHT), Color.White * .5f);

            foreach (var spaceObject in _space.Objects)
            {
                if (!_space.IsInBounds(spaceObject))
                    continue;
                if (spaceObject.IsPointOfView)
                    continue;
                if (spaceObject is Ship)
                {
                    Ship ship = spaceObject as Ship;
                    DrawMinimapBogey(spriteBatch,contentContainer, ship, width, height, xMin, yMax);
                }
                else if (spaceObject is Missile)
                {
                    spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), GetMinimapRectangle(spaceObject, width, height,xMin, yMax, 1, 1), Color.Black);
                }
            }
            DrawMinimapBogey(spriteBatch, contentContainer, _space.PointOfViewObject, width, height, xMin, yMax);
        }

        private Rectangle GetMinimapRectangle(SpaceObject spaceObject, int minimapWidth, int minimapHeight, int xMin, int yMax, int width, int height)
        {
            var xScale = (spaceObject.Orientation.Position.X / _space.Width) * minimapWidth;
            var yScale = (spaceObject.Orientation.Position.Y / _space.Height) * minimapHeight;

            var xPos = xScale + xMin + minimapWidth / 2;
            var yPos = yScale + yMax + minimapHeight / 2;

            return new Rectangle((int)xPos, (int)yPos, width, height);
        }

        private void DrawMinimapBogey(SpriteBatch spriteBatch, IContentContainer contentContainer, Ship ship, int width, int height, int xMin, int yMax)
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

            spriteBatch.Draw(contentContainer.LoadTexture("Bogey"), GetMinimapRectangle(ship, width, height, xMin, yMax, BLIP_WIDTH, BLIP_HEIGHT), null, color, ship.Orientation.Angle, new Vector2(BLIP_WIDTH, BLIP_HEIGHT), SpriteEffects.None, 1);
        }
    }
}
