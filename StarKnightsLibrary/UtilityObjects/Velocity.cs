namespace StarKnightsLibrary.UtilityObjects
{
    /// <summary>
    /// Represents a velocity in an arbitrary direction with strictly constrained minimum and maximum values
    /// </summary>
    public class Velocity
    {
        /// <summary>
        /// The minimum velocity
        /// </summary>
        public float Min { get; private set; }
        /// <summary>
        /// The maximum velocity
        /// </summary>
        public float Max { get; private set; }

        private float _current;
        /// <summary>
        /// The current velocity
        /// </summary>
        public float Current
        {
            get => _current;
            set
            {
                _current = value;
                _current = _current < Min ? Min : _current;
                _current = _current > Max ? Max : _current;
            }
        }

        /// <summary>
        /// Creates a new velocity object with the current velocity defaulted at the specified minimum
        /// </summary>
        /// <param name="min">The minimum velocity</param>
        /// <param name="max">The maximum velocity</param>
        public Velocity(float min, float max)
        {
            Min = min;
            Max = max;
            Current = min;
        }

        public override string ToString()
        {
            return Current.ToString();
        }
    }
}
