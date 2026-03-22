namespace Assets.Scripts.Interfaces
{
    public interface ITurn
    {
        int Player { get; }
        int Speed { get; }
        void ExecuteTurn();
    }
}