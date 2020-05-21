namespace StarKnightsLibrary.Triggers
{
    public class ConditionResult
    {
        public bool ConditionPassed { get; set; }
    }

    public class ConditionResult<T> : ConditionResult
    {
        public T Data { get; set; }
    }
}
