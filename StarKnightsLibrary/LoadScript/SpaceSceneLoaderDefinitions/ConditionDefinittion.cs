﻿using Microsoft.Xna.Framework;
using StarKnightsLibrary.SpaceObjects.Ships;
using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.Triggers;
using StarKnightsLibrary.UtilityObjects.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    public class ConditionDefinittion
    {
        public string Type { get; set; }
        public Dictionary<string, string> Variables { get; set; }

        public Condition<ConditionResult> GetCondition()
        {
            return Type switch
            {
                "NotShipExists" => NotShipExists,
                "NotActiveTransmission" => NotActiveTransmission,
                "ShipExists" => ShipExists,
                "ShipOutOfBounds" => ShipOutOfBounds,
                _ => throw new Exception($"Condition Type {Type} is not defined")
            };
        }

        public Condition<ConditionResult<Ship>> GetConditionShip()
        {
            return Type switch
            {
                "ShipExists" => ShipExists,
                "ShipOutOfBounds" => ShipOutOfBounds,
                _ => throw new Exception($"Condition Type {Type} is not defined")
            };
        }

        private IEnumerable<Ship> GetShips(ISpaceScene enviroment)
        {
            var enumerable = enviroment.Ships;

            if (Variables.TryGetValue("IsPlayer", out string isPlayerString) &&
                bool.TryParse(isPlayerString, out bool isPlayer))
            {
                enumerable = enumerable.Where(s => s.IsPointOfView == isPlayer);
            }
            if (Variables.TryGetValue("Location", out string locationString) &&
                locationString.TryParse(out Vector2 location) &&
                Variables.TryGetValue("Distance", out string distanceString) &&
                float.TryParse(distanceString, out float distance))
            {
                enumerable = enumerable.Where(s => (s.Orientation.Position - location).Length() <= distance);
            }
            return enumerable;
        }

        private ConditionResult NotShipExists(ISpaceScene enviroment)
        {
            return new ConditionResult
            {
                ConditionPassed = !GetShips(enviroment).Any()
            };
        }

        private ConditionResult NotActiveTransmission(ISpaceScene enviroment)
        {
            return new ConditionResult
            {
                ConditionPassed = !enviroment.ActiveTransmission
            };
        }

        private ConditionResult<Ship> ShipExists(ISpaceScene enviroment)
        {

            var ship = GetShips(enviroment).FirstOrDefault();

            return new ConditionResult<Ship>
            {
                ConditionPassed = ship != null,
                Data = ship
            };
        }

        private ConditionResult<Ship> ShipOutOfBounds(ISpaceScene enviroment)
        {

            var ship = GetShips(enviroment).FirstOrDefault();

            return new ConditionResult<Ship>
            {
                ConditionPassed = !ship.IsInBounds(),
                Data = ship
            };
        }
    }
}