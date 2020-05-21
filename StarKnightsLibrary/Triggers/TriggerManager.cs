using StarKnightsLibrary.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.Triggers
{
    public class TriggerManager
    {
        private readonly IDictionary<uint, TriggerManagerBucket> _triggers;
        private uint _nextKey;

        public TriggerManager()
        {
            _triggers = new Dictionary<uint, TriggerManagerBucket>();
            _nextKey = 1;
        }

        public uint AddTrigger<T>(Condition<T> condition, Effect<T> effect, bool recurring)
            where T : ConditionResult
        {
            var trigger = new Trigger<T>(condition, effect, _nextKey++);
            _triggers.Add(trigger.Id, new TriggerManagerBucket(trigger, recurring));
            return trigger.Id;
        }

        public void Update(ISpaceScene enviroment)
        {
            var triggerList = _triggers
                .Select(t => t.Value)
                .ToList();
            foreach (var trigger in triggerList)
            {
                if (trigger.Update(enviroment) && !trigger.Recurring)
                {
                    RemoveTrigger(trigger.Id);
                }
            }
        }

        public void RemoveTrigger(uint id)
        {
            _triggers.Remove(id);
        }

        private class TriggerManagerBucket : ITrigger
        {
            public ITrigger Trigger { get; }
            public bool Recurring { get; }

            public uint Id => Trigger.Id;

            public TriggerManagerBucket(ITrigger trigger, bool recurring)
            {
                Trigger = trigger;
                Recurring = recurring;
            }

            public bool Update(ISpaceScene enviroment)
            {
                return Trigger.Update(enviroment);
            }
        }
    }
}
