namespace TouchFall.Helper.Enums
{
    public enum StateMoveMainHero : byte
    {
        None,
        Touch,
        EndTouch,
        End
    }

    public enum StateBehaviourHero : byte
    {
        None,
        Rotate
    }

    public enum ModifyHero : byte
    {
        Drop,
        Elastic,
        Spinning,
        Magnetic,
        Repulsive,
        Square
    }

    public enum ModifyBounds : byte
    {
        None,
        Stay,
        IncreaseDistance,
        Moving
    }

    public enum GameState : byte
    {
        MainMenu,
        GamePlay,
        Pause,
        GameOver
    }

    public enum Language : byte
    {
        English,
        Russian
    }
}
