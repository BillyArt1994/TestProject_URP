namespace AsiActionEngine.RunTime
{
    public partial class ActionStateMachine
    {
        private void OnSetAnimatorFloat(string _name, float _value)
        {
            CurAnimator.SetFloat(_name, _value);
        }
        private void OnSetAnimatorInt(string _name, int _value)
        {
            CurAnimator.SetInteger(_name, _value);
        }
    }
}