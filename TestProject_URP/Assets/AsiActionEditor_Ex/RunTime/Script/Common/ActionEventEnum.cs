namespace AsiTimeLine.RunTime
{
    public enum EMoveDirType
    {
        Transform,
        Camera,
        World,
        inputDir_Cam,
    }

    public enum EContrast
    {
        Greater,
        Less,
        Equals,
        GreaterOrEquals,
        LessOrEquals
    }

    public enum EAnimFloatFor
    {
        InputToTransDir,
        GroundDir
    }

    public enum ERotType
    {
        Camera,
        MoveDir,
        LockToTargetDir
    }

    public enum EOnMoveDirType
    {
        MoveDir,
        Transform,
        Camera,
        World,
    }

    public enum EPriority
    {
        Height,
        Normal,
        Lower
    }

    public enum EInteraetPointType
    {
        LockTarget,
        HookLock,
        InteractPoint,
    }
}