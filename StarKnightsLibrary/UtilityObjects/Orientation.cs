using Microsoft.Xna.Framework;
using System;

namespace StarKnightsLibrary.UtilityObjects
{
    /// <summary>
    /// Represents the location and direction of an arbitrary object
    /// </summary>
    public class Orientation
    {
        private float _angle;
        /// <summary>
        /// The angle of orientation in radians(0-2𝜋)
        /// </summary>
        public float Angle
        {
            get => _angle;
            set
            {
                _angle = value % MathHelper.TwoPi;
                _angle = _angle < 0 ? _angle + MathHelper.TwoPi : _angle;
            }
        }
        /// <summary>
        /// The position of the orientation in the Cartesian plane
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Creates a new orientation object with the given location and angle
        /// </summary>
        /// <param name="x">The x coordinate of position</param>
        /// <param name="y">The y coordinate of position</param>
        /// <param name="angle">The angle of orientation</param>
        public Orientation(float x, float y, float angle)
        {
            Position = new Vector2(x, y);
            Angle = angle;
        }

        /// <summary>
        /// Moves the position the specified distance in the direction of angle
        /// </summary>
        /// <param name="distance"></param>
        public void Move(float distance)
        {
            var newPosition = Position;
            newPosition.X += (float)Math.Sin(Angle) * distance;
            newPosition.Y -= (float)Math.Cos(Angle) * distance;
            Position = newPosition;
        }

        public void Reverse()
        {
            Angle = Angle - MathHelper.Pi;
        }
    }
}
