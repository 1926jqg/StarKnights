using Microsoft.Xna.Framework.Graphics;
using StarKnightsLibrary.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    public class SpaceSceneDefinition : ISceneDefinition
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<ShipDefinition> Ships { get; set; }
        public List<TriggerDefinition> Triggers { get; set; }

        public IScene Build(Viewport viewport)
        {
            var scene = new SpaceScene(viewport, Width, Height);
            foreach (var ship in Ships?.ToList() ?? new List<ShipDefinition>())
            {
                ship.Build(scene);
            }
            foreach (var trigger in Triggers?.ToList() ?? new List<TriggerDefinition>())
            {
                trigger.Build(scene);
            }

            return scene;
        }
    }
}
