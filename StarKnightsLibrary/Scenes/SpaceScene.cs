using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.GameObjects.Ships;
using StarKnightsLibrary.GameObjects.Ships.Deciders;
using StarKnightsLibrary.Transmissions;
using StarKnightsLibrary.Triggers;
using StarKnightsLibrary.UI;
using StarKnightsLibrary.UtilityObjects;
using System.Collections.Generic;

namespace StarKnightsLibrary.Scenes
{
    public class SpaceScene : ISpaceScene
    {
        private readonly Space _space;
        private readonly Hud _hud;
        private readonly ITransmissionManager _transmissionManager;
        private readonly TriggerManager _triggerManager;
        private readonly Vector2 _screenposSpace;
        private readonly UserInterface _userInterface;
        public ulong Ticks => _space.Ticks;

        public bool IsOver => _space.PointOfViewObject == null;

        public IEnumerable<Ship> Ships => _space.Ships;

        public SpaceScene(Viewport viewport, int width, int height, int outOfBoundsLimit = 100)
        {
            _space = new Space(width, height, outOfBoundsLimit);
            _screenposSpace.X = viewport.Width / 2f;
            _screenposSpace.Y = viewport.Height - 250;
            _triggerManager = new TriggerManager();
            _hud = new Hud(_space, viewport.Width, 200);
            _transmissionManager = _hud.TransmissionManager;
            _userInterface = new UserInterface()
            {
                ScreenHeight = viewport.Height,
                ScreenWidth = viewport.Width,
                ShowCursor = false
            };
            UserInterface.Active = _userInterface;
        }

        public void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, GetMatrix());
            _space.Draw(spriteBatch, contentContainer);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            var playerShip = _space.PointOfViewObject;
            playerShip.Draw(spriteBatch, contentContainer);
            _hud.Draw(spriteBatch, contentContainer, _userInterface);
            spriteBatch.End();

            _userInterface.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _triggerManager.Update(this);
            _transmissionManager.Update(this);
            _userInterface.Update(gameTime);
            _space.Update();
        }

        private Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_space.PointOfViewObject?.Orientation.Position ?? new Vector2(), 0))
                * Matrix.CreateRotationZ(-_space.PointOfViewObject?.Orientation.Angle ?? 0)
                * Matrix.CreateTranslation(new Vector3(_screenposSpace, 0));
        }

        public void AddNonPlayerShip<TFollow, TTarget>(int x, int y, float angle, int width, int height, int team)
            where TFollow : IFollowDecider, new()
            where TTarget : ITargetDecider, new()
        {
            _space.AddNonPlayerShip<TFollow, TTarget>(x, y, angle, width, height, team);
        }

        public void AddPlayerShip(int x, int y, float angle, int width, int height, int team)
        {
            _space.AddPlayerShip(x, y, angle, width, height, team);
        }

        public void AddTransmission(ITransmission transmission, ulong duration)
        {
            _transmissionManager.AddTransmission(transmission, duration);
        }

        public uint AddTrigger<T>(Condition<T> condition, Effect<T> effect, bool recurring)
            where T : ConditionResult
        {
            return _triggerManager.AddTrigger(condition, effect, recurring);
        }

        public IScene Next
        {
            get
            {
                return null;
            }
        }
    }
}
