using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.GameObjects.Projectiles.Missiles;
using StarKnightsLibrary.GameObjects.Ships;
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
            _paragraph = new RichParagraph("", scale: 1f, offset: new Vector2(-10, -10), size: new Vector2(250, 200));
            _paragraph.Padding = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer, UserInterface ui)
        {
            TransmissionManager.Draw(ui, contentContainer);
            DrawInfo(ui, contentContainer);
            DrawMinimap(spriteBatch, contentContainer, 10, 10, _height, _height);
        }

        private void DrawInfo(UserInterface ui, IContentContainer contentContainer)
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
                    offset: new Vector2(_width - panelWidth - 10, ui.ScreenHeight - panelHeight - 13));
                _infoPanel.Opacity = 128;
                ui.AddEntity(_infoPanel);
                _infoPanel.AddChild(_paragraph);
            }

            _paragraph.Text = builder.ToString();
        }

        private void DrawMinimap(SpriteBatch spriteBatch, IContentContainer contentContainer, int leftBorder, int bottomBorder, int width, int height)
        {
            Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

            int xMin = leftBorder;
            int yMax = viewport.Height - height - bottomBorder;
            int blipWidth = 8;
            int blipHeight = 8;

            spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), new Rectangle(xMin - blipWidth / 2, yMax - blipHeight / 2, width + blipWidth, height + blipHeight), Color.White * .5f);


            foreach (var spaceObject in _space.Objects)
            {
                if (!_space.IsInBounds(spaceObject))
                    continue;
                var xScale = (spaceObject.Orientation.Position.X / _space.Width) * width;
                var yScale = (spaceObject.Orientation.Position.Y / _space.Height) * height;

                var xPos = xScale + xMin + width / 2;
                var yPos = yScale + yMax + height / 2;

                Rectangle rect = new Rectangle((int)xPos, (int)yPos, blipWidth, blipHeight);

                if (spaceObject is Ship)
                {
                    Ship ship = spaceObject as Ship;
                    Color color;
                    if (spaceObject.IsPointOfView)
                    {
                        color = Color.Green;
                    }
                    else if (ship.Team == _space.PointOfViewObject.Team)
                    {
                        color = Color.Blue;
                    }
                    else
                    {
                        color = Color.Red;
                    }

                    spriteBatch.Draw(contentContainer.LoadTexture("Bogey"), rect, null, color, spaceObject.Orientation.Angle, new Vector2(blipWidth, blipHeight), SpriteEffects.None, 1);
                }
                else if (spaceObject is Missile)
                {
                    spriteBatch.Draw(contentContainer.LoadTexture("Pixel"), new Rectangle((int)xPos, (int)yPos, 1, 1), Color.Black);
                }
            }
        }
    }
}
