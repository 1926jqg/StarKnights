using System;

namespace StarKnightsLibrary.ConversationObjects
{
    public class ConversationNodeOption : IEquatable<ConversationNodeOption>
    {
        public string Key { get; }
        public string Description { get; }

        public ConversationNodeOption(string key, string description)
            : this(key)
        {
            Description = description;
        }

        private ConversationNodeOption(string key)
        {
            Key = key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key);
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                ConversationNodeOption c => Equals(c),
                _ => false
            };
        }

        public bool Equals(ConversationNodeOption other)
        {
            return other.Equals(Key);
        }

        public static implicit operator ConversationNodeOption(string s) => new ConversationNodeOption(s);
    }
}
