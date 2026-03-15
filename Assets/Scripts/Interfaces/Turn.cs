namespace Assets.Scripts.Interfaces
{
    public interface ITurn
    {
        int Player { get; }
        void ExecuteTurn();
    }
}