namespace AsiActionEngine.RunTime
{
    public class CheckOnHit : IInterruptCondition
    {
        public int InterruptType => -(int)EInterruptTypeInternal.EIT_CheckOnHit;
        public bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart)
        {
            return true;
        }

        public IInterruptCondition Clone()
        {
            return this;
        }
    }
}