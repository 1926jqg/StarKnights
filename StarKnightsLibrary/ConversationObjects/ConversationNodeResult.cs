namespace StarKnightsLibrary.ConversationObjects
{
    public class ConversationNodeResult
    {
        public ConversationNodeOption NextNode { get; }
        public bool OperationSuccess { get; }

        public ConversationNodeResult()
        {
            OperationSuccess = false;
        }
        public ConversationNodeResult(ConversationNodeOption nextNode)
        {
            OperationSuccess = true;
            NextNode = nextNode;
        }
    }
}
