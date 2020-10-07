using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.Triggers;

namespace StarKnightsLibrary.UnitTests.TestDoubles.Spies
{
    public class EffectSpy<T>
        where T : ConditionResult
    {
        public bool ActWasCalled
        {
            get
            {
                return TimesActCalled > 0;
            }
        }
        public int TimesActCalled { get; private set; } = 0;
        public T Result { get; private set; }

        public void Act(ISpaceScene scene, T result)
        {
            Result = result;
            TimesActCalled++;
        }
    }
}
