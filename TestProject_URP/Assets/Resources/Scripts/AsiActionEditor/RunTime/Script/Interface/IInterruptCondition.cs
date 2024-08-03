namespace AsiActionEngine.RunTime
{
    public interface IInterruptCondition
    {
        int InterruptType { get; }
        bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart);
        IInterruptCondition Clone();
    }
}