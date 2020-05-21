using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.UtilityObjects;

namespace StarKnightsLibrary.Scenes
{
    public interface IScene
    {
        ulong Ticks { get; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, IContentContainer contentContainer);
        bool IsOver { get; }

        IScene Next { get; }
    }
}
