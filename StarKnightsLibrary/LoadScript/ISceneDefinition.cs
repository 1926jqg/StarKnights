using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.Scenes;

namespace StarKnightsLibrary.LoadScript
{
    public interface ISceneDefinition
    {
        IScene Build(Viewport viewport);
    }
}
