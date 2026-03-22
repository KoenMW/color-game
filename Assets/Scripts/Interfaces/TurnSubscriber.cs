namespace Assets.Scripts.Interfaces
{
    public interface ISubscriber
    {
        string Name { get; }
        void OnTurnExecuted(Turn turn);
    }
}