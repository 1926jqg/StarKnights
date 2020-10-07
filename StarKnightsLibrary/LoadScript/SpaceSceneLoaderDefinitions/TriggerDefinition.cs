using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.Scenes;
using System;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    public class TriggerDefinition
    {
        public bool Recurring { get; set; }
        public ConditionDefinittion Condition { get; set; }
        public EffectDefinition Effect { get; set; }

        public void Build(SpaceScene scene)
        {
            switch (Condition.Type)
            {
                case "ShipExists":
                    scene.AddTrigger(Condition.GetConditionShip(), Effect.GetEffect<Ship>(), Recurring);
                    break;
                case "NotShipExists":
                    scene.AddTrigger(Condition.GetCondition(), Effect.GetEffect(), Recurring);
                    break;
                default:
                    throw new Exception($"Condition type {Condition.Type} is not defined.");
            }
        }

    }
}
