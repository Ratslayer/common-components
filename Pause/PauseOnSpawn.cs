namespace BB
{
    public sealed class PauseOnSpawn : PushValueOnSpawn<Paused, bool>
    {
        public override bool Value => true;
    }
}
