using System;
using System.Collections.Generic;

namespace StarKnightsLibrary.ConversationObjects
{
    public interface IConversationNode
    {
        IEnumerable<ConversationNodeOption> Options { get; }

        string Description { get; }
    }

    public class ConversationNode : IConversationNode
    {
        private readonly Dictionary<ConversationNodeOption, string> _optionsResultsMapping;
        public IEnumerable<ConversationNodeOption> Options => _optionsResultsMapping.Keys;

        public string Description { get; }

        public ConversationNode(string description)
        {
            _optionsResultsMapping = new Dictionary<ConversationNodeOption, string>();
            Description = description;
        }

        public void AddOption(string key, string description, string result)
        {
            if (_optionsResultsMapping.ContainsKey(key))
                throw new InvalidOperationException($"Option \"{key}\" could not be added. It already exists in the ConversationNode.");
            _optionsResultsMapping.Add(new ConversationNodeOption(key, description), result);
        }

        public ConversationNodeResult ChooseOption(ConversationNodeOption option)
        {
            if (!_optionsResultsMapping.TryGetValue(option, out string result))
                return new ConversationNodeResult();
            return new ConversationNodeResult(result);
        }
    }
}
