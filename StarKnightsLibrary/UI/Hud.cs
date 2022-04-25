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
        private readonly Minimap _minimap;
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

            _minimap = new Minimap(_space, 10, 10, _height, _height);
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer, UserInterface ui)
        {
            TransmissionManager.Draw(ui, contentContainer);
            DrawInfo(ui);
            _minimap.Draw(spriteBatch, contentContainer);
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
    }
}
