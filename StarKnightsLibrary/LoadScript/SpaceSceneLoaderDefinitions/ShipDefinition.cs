using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.SpaceObjects.Ships.Deciders;
using StarKnightsLibrary.Scenes;
using System.Runtime.Serialization;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    [DataContract]
    public class ShipDefinition
    {
        [DataMember(Name = "x")]
        public string XString { get; set; }
        [DataMember(Name = "y")]
        public string YString { get; set; }
        [DataMember(Name = "angle")]
        public string AngleString { get; set; }
        [DataMember(Name = "width")]
        public string WidthString { get; set; }
        [DataMember(Name = "height")]
        public string HeightString { get; set; }
        [DataMember(Name = "team")]
        public string TeamString { get; set; }
        [DataMember(Name = "count")]
        public string CountString { get; set; }
        [DataMember(Name = "player")]
        public bool Player { get; set; }

        public int X
        {
            get => SceneLoader.GetIntFromString(XString);
            set => XString = value.ToString();
        }
        public int Y
        {
            get => SceneLoader.GetIntFromString(YString);
            set => YString = value.ToString();
        }
        public float Angle
        {
            get => SceneLoader.GetFloatFromString(AngleString);
            set => AngleString = value.ToString();
        }
        public int Width
        {
            get => SceneLoader.GetIntFromString(WidthString);
            set => WidthString = value.ToString();
        }
        public int Height
        {
            get => SceneLoader.GetIntFromString(HeightString);
            set => HeightString = value.ToString();
        }
        public int Team
        {
            get => SceneLoader.GetIntFromString(TeamString);
            set => TeamString = value.ToString();
        }

        public int Count
        {
            get
            {
                var count = SceneLoader.GetIntFromString(CountString);
                return string.IsNullOrEmpty(CountString) ? 1 : count;
            }
            set => TeamString = value.ToString();
        }

        public void Build(SpaceScene scene)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Player)
                {
                    scene.AddPlayerShip(X, Y, Angle, Height, Width, Team);
                }
                else
                {
                    scene.AddNonPlayerShip<IntelligentFollowDecider, TeamTargetDecider>(X, Y, Angle, Height, Width, Team);
                }
            }
        }
    }
}