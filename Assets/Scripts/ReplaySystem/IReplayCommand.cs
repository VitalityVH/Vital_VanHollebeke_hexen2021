namespace Hexen.ReplaySystem
{
    public interface IReplayCommand
    {
        void Forward(); //Execute

        void Backward(); //undo

    }
}