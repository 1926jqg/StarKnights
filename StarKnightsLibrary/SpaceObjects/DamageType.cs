using System;
using System.Runtime.Serialization;

namespace StarKnightsLibrary.SpaceObjects
{
    public enum DamageType
    {
        Concussive,
        Thermal
    }

    [Serializable]
    public class DamageTypeUnexpectedException : Exception
    {
        public DamageTypeUnexpectedException(DamageType type)
            : base(string.Format("{0} was not an expected damage type", type.ToString())) { }

        protected DamageTypeUnexpectedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
