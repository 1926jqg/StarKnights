using StarKnightsLibrary.Scenes;

namespace StarKnightsLibrary.Triggers
{
    public delegate T Condition<out T>(ISpaceScene enviroment) where T : ConditionResult;
    public delegate void Effect<in T>(ISpaceScene enviroment, T result) where T : ConditionResult;

    public interface ITrigger
    {
        /// <summary>
        /// Runs the trigger, attempting to run the Action if the Condition resolves
        /// </summary>
        /// <param name="enviroment">The enviroment context that this trigger is running in</param>
        /// <returns>True if the conditions passed and the Action executed, false otherwise</returns>
        bool Update(ISpaceScene enviroment);
        uint Id { get; }
    }

    public class Trigger<T> : ITrigger
        where T : ConditionResult
    {
        private readonly Condition<T> _condition;
        private readonly Effect<T> _effect;
        public uint Id { get; private set; }

        public Trigger(Condition<T> condition, Effect<T> effect, uint id = 0)
        {
            _condition = condition;
            _effect = effect;
            Id = id;
        }

        public bool Update(ISpaceScene enviroment)
        {
            var result = _condition(enviroment);
            if (!result.ConditionPassed)
                return false;
            _effect(enviroment, result);
            return true;
        }
    }
}
