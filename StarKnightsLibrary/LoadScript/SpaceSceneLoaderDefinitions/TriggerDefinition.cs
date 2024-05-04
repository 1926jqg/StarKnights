using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.Scenes;
using System;
using System.Collections.Generic;
using StarKnightsLibrary.Triggers;
using System.Linq;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    public class TriggerDefinition
    {
        public bool Recurring { get; set; }
        public IList<ConditionDefinittion> Conditions { get; set; }
        public EffectDefinition Effect { get; set; }

        public void BuildSingleCondition(ConditionDefinittion condition, SpaceScene scene)
        {
            switch (condition.Type)
            {
                case "ShipExists":
                case "ShipOutOfBounds":
                    scene.AddTrigger(condition.GetConditionShip(), Effect.GetEffect<Ship>(), Recurring);
                    break;
                case "NotShipExists":
                case "NotActiveTransmission":
                    scene.AddTrigger(condition.GetCondition(), Effect.GetEffect(), Recurring);
                    break;
                default:
                    throw new Exception($"Condition type {condition.Type} is not defined.");

            }
        }

        public void Build(SpaceScene scene)
        {
            var count = Conditions.Count;
            if(count <= 0)
            {
                throw new Exception($"No conditions were defined");
            }
            else if(count == 1)
            {
                BuildSingleCondition(Conditions.First(), scene);
            }
            scene.AddTrigger(And(), Effect.GetEffect(), Recurring);
        }

        private Condition<ConditionResult> And()
        {
            var conditions = Conditions.Select(c => c.GetCondition()).ToList();
            return x => new ConditionResult
            {
                ConditionPassed = conditions.All(c => c(x).ConditionPassed)
            };
        }
    }
}
