﻿using StarKnightsLibrary.GameFlow;

namespace StarKnightsLibrary.GameObjects.Ships
{
    public interface IShipDecider : IDecider<Ship>
    {
        bool IsInBounds(Ship ship);
        void Initialize(Space space);
    }
}
