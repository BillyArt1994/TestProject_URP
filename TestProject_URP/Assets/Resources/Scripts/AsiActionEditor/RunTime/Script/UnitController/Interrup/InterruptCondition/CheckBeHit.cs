namespace AsiActionEngine.RunTime
{
    public class CheckBeHit : IInterruptCondition
    {
        public int InterruptType => -(int)EInterruptTypeInternal.EIT_CheckBeHit;
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