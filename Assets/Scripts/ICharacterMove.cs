public interface ICharacterMove
{
    string Name { get; }
    string Description { get; }
    ColorEnum Type { get; }
    int Power { get; }
    int Accuracy { get; }
    int MaxPP { get; }
}