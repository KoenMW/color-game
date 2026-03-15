namespace Assets.Scripts.Interfaces
{
    public interface ISubscriber
    {
        string Name { get; }
        void OnTurnExecuted(ITurn turn);
    }
}