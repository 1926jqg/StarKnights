using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.LoadScript;
using StarKnightsLibrary.UtilityObjects;

namespace StarKnightsLibrary.Scenes
{
    public class StartMenuScene : IScene
    {
        private readonly UserInterface _userInterface;
        private readonly Viewport _viewport;

        public ulong Ticks { get; private set; } = 0;
        public IScene Next { get; private set; }
        public bool IsOver { get; private set; } = false;

        public StartMenuScene(Viewport viewport)
        {
            _userInterface = new UserInterface()
            {
                ScreenHeight = viewport.Height,
                ScreenWidth = viewport.Width,
                ShowCursor = true
            };
            UserInterface.Active = _userInterface;

            var mainPanel = new Panel(new Vector2(400, 200), anchor: Anchor.BottomCenter, offset: new Vector2(0, 100));
            var button = new Button("New Game");
            button.OnClick += OnClick;
            mainPanel.AddChild(button);

            _userInterface.AddEntity(mainPanel);
            _viewport = viewport;
        }

        private void OnClick(Entity entity)
        {
            Next = SceneLoader.Load(_viewport, "Scenes/test_scene.json");
            IsOver = true;
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            Ticks++;
            _userInterface.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _userInterface.Update(gameTime);
        }
    }
}
