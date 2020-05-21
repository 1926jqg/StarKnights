using StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions;

namespace StarKnightsLibrary.LoadScript
{
    public class SceneDefinitionContainer
    {
        public SpaceSceneDefinition SpaceScene { get; set; }

        public ISceneDefinition Get()
        {
            return SpaceScene;
        }
    }
}
